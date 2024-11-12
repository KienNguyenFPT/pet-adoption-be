using BusinessLogicLayer;
using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.DonationDTOs;
using BusinessObjects.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace PetAdoptionApp_Prn231_Group9.Controllers
{
    public class DonationController : BaseController
    {
        private readonly IDonationServices _donationService;
        private readonly IUnitOfWork _unitOfWork;
        public DonationController(IDonationServices donationService, IUnitOfWork unitOfWork)
        {
            _donationService = donationService;
            _unitOfWork = unitOfWork;
        }

        // POST: api/Donation/CreatePayment
        [Authorize(Roles = nameof(Role.User))]
        [HttpPost("CreatePayment/{ShelterId}")]
        public async Task<IActionResult> CreatePayment([FromBody]DonationDTOs donationDTO, Guid ShelterId)
        {
            if (donationDTO == null || !donationDTO.Money.HasValue || donationDTO.Money <= 0)
            {
                return BadRequest("Invalid donation details.");
            }

            try
            {
                var result = await _donationService.CreateDonationPayments(donationDTO, ShelterId);
                return Ok(result); // Returns the donation details with payment transaction info
            }
            catch (Exception ex)
            {
                // Handle and log the error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Donation/CapturePayment
        [Authorize(Roles = nameof(Role.User))]
        [HttpPost("CapturePayment/{transactionId}")]
        public async Task<IActionResult> CapturePayment(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
            {
                return BadRequest("Transaction ID is required.");
            }

            try
            {
                var result = await _donationService.CaptureDonationPayment2ndVersion(transactionId);
                return Ok(result); // Returns the updated donation details
            }
            catch (Exception ex)
            {
                // Handle and log the error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = nameof(Role.User))]
        [HttpGet("GetDonationByUserId/{UserId}")]
        public async Task<IActionResult> GetDonationByUser(Guid UserId)
        {
            var result = await _donationService.GetAllDonationByUserId(UserId);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }


        [Authorize(Roles = nameof(Role.User) + "," + nameof(Role.Staff) + "," + nameof(Role.Administrator))]
        [HttpGet("getDonationId/{id}")]
        public async Task<IActionResult> GetDonationById(Guid id)
        {
            var result = await _donationService.GetDonationById(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }

        }
        [Authorize(Roles = nameof(Role.Staff) + "," + nameof(Role.Administrator))]
        [HttpGet("GetAllDonation")]
        public async Task<IActionResult> GetDonations()
        {
            var result = await _donationService.GetAllDonations();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }

        }
        [Authorize(Roles = nameof(Role.User))]
        [HttpGet("GetById/{UserId}")]
        public async Task<IActionResult> GetDonationWithPaymentStatus(Guid UserId)
        {
            var result = await _donationService.GetDonationByUserIdWithPaymentStatus(UserId);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();

            }
        }

    }
}