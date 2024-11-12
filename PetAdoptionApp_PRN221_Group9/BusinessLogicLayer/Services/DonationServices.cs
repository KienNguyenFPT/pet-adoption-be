using AutoMapper;
using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.DonationDTOs;
using BusinessObjects;
using DataAccessObjects.ServicesResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class DonationServices : IDonationServices
    {
        private readonly PayPalHttpClient _client;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimServices _claimServices;
        private readonly ICurrentTimeServices _currentTimeServices;

        public DonationServices(PayPalHttpClient client, IUnitOfWork unitOfWork
            ,IMapper mapper, IClaimServices claimServices, ICurrentTimeServices currentTimeServices)
        {
            _client = client;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimServices = claimServices;
            _currentTimeServices = currentTimeServices;
        }

        public async Task<ServicesResponses<DonationDTOs>> CaptureDonationPayment(string transactionId)
        {
            var responses = new ServicesResponses<DonationDTOs>();

            try
            {
                if (string.IsNullOrEmpty(transactionId))
                {
                    responses.Success = false;
                    responses.Message = "Transaction ID cannot be null or empty.";
                    return responses;
                }

                // Get PayPal Access Token
                var token = await GetPaypalAccessToken();
                if (string.IsNullOrEmpty(token))
                {
                    responses.Success = false;
                    responses.Message = "Failed to retrieve PayPal access token.";
                    return responses;
                }

                // Check if the order is approved before capturing
                var getOrderRequest = new OrdersGetRequest(transactionId);
                var orderResponse = await _client.Execute(getOrderRequest);
                if (orderResponse.StatusCode != HttpStatusCode.OK)
                {
                    responses.Success = false;
                    responses.Message = "Failed to retrieve order status.";
                    return responses;
                }

                var orderResult = orderResponse.Result<Order>();
                if (orderResult.Status != "APPROVED")
                {
                    responses.Success = false;
                    responses.Message = "The order has not been approved for payment capture.";
                    return responses;
                }

                // Capture the payment
                var request = new OrdersCaptureRequest(transactionId);
                request.Prefer("return=representation");
                request.RequestBody(new OrderActionRequest());
                var response = await _client.Execute(request);

                // Check response status
                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
                {
                    responses.Success = false;
                    responses.Message = $"Payment capture failed with status: {response.StatusCode}.";
                    return responses;
                }

                // Process the response
                var result = response.Result<Order>();
                var donation = await _unitOfWork._donationRepo.GetTransactionId(transactionId);
                if (donation == null)
                {
                    responses.Success = false;
                    responses.Message = "Donation not found.";
                    return responses;
                }

                // Update donation record in the database
                donation.PaymentStatus = "Completed";
                donation.TransactionId = result.Id;
                _unitOfWork._donationRepo.Update(donation);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;

                responses.Success = isSuccess;
                responses.Message = isSuccess ? "Payment captured successfully." : "Failed to capture payment.";
                responses.Data = isSuccess ? _mapper.Map<DonationDTOs>(donation) : null;
            }
            catch (PayPalHttp.HttpException httpEx)
            {
                responses.Success = false;
                responses.Message = $"PayPal error: {httpEx.Message}";
            }
            catch (Exception ex)
            {
                responses.Success = false;
                responses.Message = $"An error occurred: {ex.Message}";
            }

            return responses;
        }
        

        public async Task<ServicesResponses<DonationDTOs>> CaptureDonationPayment2ndVersion(string transactionId)
        {
            var responses = new ServicesResponses<DonationDTOs>();
            IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").
                    Build();
            try
            {
                if (string.IsNullOrEmpty(transactionId))
                {
                    responses.Success = false;
                    responses.Message = "Transaction ID cannot be null or empty.";
                    return responses;
                }

                // Step 1: Get PayPal Access Token dynamically
                string token = await GetPaypalAccessToken();
                if (string.IsNullOrEmpty(token))
                {
                    responses.Success = false;
                    responses.Message = "Failed to retrieve PayPal access token.";
                    return responses;
                }

                // Step 2: Prepare request for Capturing Payment
                string captureUrl = $"{configuration["Paypal:Url"]}/v2/checkout/orders/{transactionId}/capture";

                using (var client = new HttpClient())
                {
                    // Add Authorization header with the Bearer token
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                    // Step 3: Create the body for capture request (PayPal API requires empty body for capture)
                    var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                    // Step 4: Send the request to PayPal Capture API
                    var response = await client.PostAsync(captureUrl, content);

                    // Check if the request was successful
                    if (!response.IsSuccessStatusCode)
                    {
                        responses.Success = false;
                        responses.Message = $"Failed to capture the payment. Status code: {response.StatusCode}.";
                        return responses;
                    }

                    // Step 5: Parse the response from PayPal
                    var strResponse = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);

                    // Step 6: Handle response from PayPal
                    if (jsonResponse != null && jsonResponse["status"]?.ToString() == "COMPLETED")
                    {
                        // If the capture is successful, update the donation record in the database
                        var donation = await _unitOfWork._donationRepo.GetTransactionId(transactionId);
                        if (donation == null)
                        {
                            responses.Success = false;
                            responses.Message = "Donation not found.";
                            return responses;
                        }

                        // Step 7: Update donation record in the database with captured payment status
                        donation.PaymentStatus = "Completed";
                        donation.TransactionId = transactionId;
                        _unitOfWork._donationRepo.Update(donation);
                        var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;

                        // Return success or failure based on database update
                        responses.Success = isSuccess;
                        responses.Message = isSuccess ? "Payment captured successfully." : "Failed to capture payment.";
                        responses.Data = isSuccess ? _mapper.Map<DonationDTOs>(donation) : null;
                    }
                    else
                    {
                        responses.Success = false;
                        responses.Message = "Payment capture failed. PayPal response was not successful.";
                    }
                }
            }
            catch (Exception ex)
            {
                // Catch and log any errors
                responses.Success = false;
                responses.Message = $"An error occurred: {ex.Message}";
            }

            return responses;
        }

        public async Task<ServicesResponses<string>> CreateDonationPayments(DonationDTOs donationDTOs, Guid ShelterId)
        {
            var responses = new ServicesResponses<string>();
            var donation = _mapper.Map<Donation>(donationDTOs);

            try
            {
                // Get the PayPal access token
                var accessToken = await GetPaypalAccessToken();
                if (string.IsNullOrEmpty(accessToken))
                {
                    responses.Success = false;
                    responses.Message = "Failed to retrieve PayPal access token.";
                    return responses;
                }

                // Create the order request
                var request = new OrdersCreateRequest();
                request.Prefer("return=representation");
                request.RequestBody(new OrderRequest()
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PurchaseUnitRequest>
            {
                new PurchaseUnitRequest()
                {
                    AmountWithBreakdown = new AmountWithBreakdown()
                    {
                        CurrencyCode = "USD",
                        Value = donation.Money.Value.ToString()
                    }
                }
            }
                });

                // Add the authorization header with the access token
                request.Headers.Add("Authorization", $"Bearer {accessToken}");

                // Execute the order creation request
                var response = await _client.Execute(request);
                var result = response.Result<Order>();

                // Check if the request was successful
                if (response.StatusCode != HttpStatusCode.Created)
                {
                    responses.Success = false;
                    responses.Message = $"Failed to create PayPal order: {response.StatusCode}";
                    return responses;
                }

                // Extract the approval URL
                var approvalUrl = result.Links.FirstOrDefault(link => link.Rel == "approve")?.Href;
                if (string.IsNullOrEmpty(approvalUrl))
                {
                    responses.Success = false;
                    responses.Message = "Approval URL not found.";
                    return responses;
                }

                // Update donation with PayPal transaction details
                donation.TransactionId = result.Id;
                donation.PaymentStatus = "Pending";

                var getUserId = _claimServices.GetCurrentUserId;

                if (getUserId == null)
                {
                    responses.Success = false;
                    responses.Message = "You need to login first to use this function.";
                    return responses;
                }
                donation.UserId = getUserId;
                donation.Id = Guid.NewGuid();
                donation.Date = _currentTimeServices.GetCurrentTime();
                donation.ShelterId = ShelterId;
                // Add the donation to the database
                await _unitOfWork._donationRepo.AddAsync(donation);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;

                if (isSuccess)
                {
                    responses.Success = true;
                    responses.Message = "Create donation successfully. Redirect to PayPal for approval.";
                    responses.Data = approvalUrl; // Return the approval URL
                }
                else
                {
                    responses.Success = false;
                    responses.Message = "Fail to create donation.";
                }

                return responses;
            }
            catch (Exception ex)
            {
                // Log the error and provide a more detailed message
                throw new Exception($"Error while creating donation payment: {ex.Message}");
            }
        }

        public async Task<ServicesResponses<IEnumerable<GetDonationDTO>>> GetAllDonationByUserId(Guid UserId)
        {
            var response = new ServicesResponses<IEnumerable<GetDonationDTO>>();
            try
            {
                UserId = _claimServices.GetCurrentUserId;
                if(UserId == null)
                {
                    response.Success = false;
                    response.Message = "You need to login first to use this function";
               
                }
                var result = await _unitOfWork._donationRepo.GetDonationByUserId(UserId);
                if (result == null)
                {
                    response.Success = false;
                    response.Message = "Failed to retrieve data";
                }else
                {
                    response.Success= true;
                    response.Message = "Retrieve data successfully";
                    response.Data = _mapper.Map<IEnumerable<GetDonationDTO>>(result);
                }

            }catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message};
            }
            return response;

        }

        public async Task<ServicesResponses<IEnumerable<GetDonationDTO>>> GetAllDonations()
        {
            var response = new ServicesResponses<IEnumerable<GetDonationDTO>>();
            try
            {
                var getUser = _claimServices.GetCurrentUserId;
                if (getUser == null)
                {
                    response.Success = false;
                    response.Message = "Please login to use this function";
                }



                var result = await _unitOfWork._donationRepo.GetAllAsync();
                if (result == null)
                {
                    response.Success = false;
                    response.Message = "Failed To Retrive Data";
                }
                else
                {
                    response.Success = true;
                    response.Message = "Retrieve data successfully";
                    response.Data = _mapper.Map<IEnumerable<GetDonationDTO>>(result);
                }








            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServicesResponses<GetDonationDTO>> GetDonationById(Guid Id)
        {
            var response = new ServicesResponses<GetDonationDTO>();
            try
            {
                var getUser = _claimServices.GetCurrentUserId;
                if (getUser == null)
                {
                    response.Success = false;
                    response.Message = "Please login to use this function";
                }



                var result = await _unitOfWork._donationRepo.GetByIdAsync(Id);
                if (result == null)
                {
                    response.Success = false;
                    response.Message = "Failed To Retrive Data";
                }else
                {
                    response.Success = true;
                    response.Message = "Retrieve data successfully";
                    response.Data = _mapper.Map<GetDonationDTO>(result);
                }
                

                





            }catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;

        }

        public async Task<ServicesResponses<GetDonationDTO>> GetDonationByUserIdWithPaymentStatus(Guid Id)
        {
            var response = new ServicesResponses<GetDonationDTO>();
            try
            {
                var getUser = _claimServices.GetCurrentUserId;
                if (getUser == null)
                {
                    response.Success = false;
                    response.Message = "Please login to use this function";
                }



                var result = await _unitOfWork._donationRepo.GetDonationByUserIDWithPaymentStatus(Id);
                if (result == null)
                {
                    response.Success = false;
                    response.Message = "Failed To Retrive Data";
                }
                else
                {
                    if (result.PaymentStatus == "Pending")
                    {
                        response.Success = true;
                        response.Message = "Retrieve data successfully";
                        response.Data = _mapper.Map<GetDonationDTO>(result);
                    } else
                    {
                        response.Success = false;
                        response.Message = "This function need to be appeared as Pending status";
                    }
                }








            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        private async Task<string> GetPaypalAccessToken()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var PaypalUrl = configuration["Paypal:Url"];
            var PaypalClientId = configuration["Paypal:ClientId"];
            var PaypalSecret = configuration["Paypal:Secret"];

            string accessToken = "";
            string url = $"{PaypalUrl}/v1/oauth2/token"; // Ensure PaypalUrl is correctly set

            using (var client = new HttpClient())
            {
                string credentials64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{PaypalClientId}:{PaypalSecret}"));

                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials64);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded")
                };

                try
                {
                    // Send the request asynchronously
                    var response = await client.SendAsync(requestMessage);

                    if (response.IsSuccessStatusCode)
                    {
                        var strResponse = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JsonNode.Parse(strResponse);

                        if (jsonResponse != null)
                        {
                            accessToken = jsonResponse["access_token"]?.ToString() ?? "";
                        }
                    }
                    else
                    {
                        // Handle non-success status codes here
                        throw new Exception($"Failed to get access token. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the error as needed
                    throw new Exception($"Error while retrieving PayPal access token: {ex.Message}");
                }
            }

            return accessToken;
        }





    }
}
