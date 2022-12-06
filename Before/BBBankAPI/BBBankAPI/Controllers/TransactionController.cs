﻿using AutoWrapper.Wrappers;
using Entities.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace BBBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpGet]
        [Route("GetLast12MonthBalances")]
        public async Task<ApiResponse> GetLast3MonthBalances()
        {
            try
            {
                var res = await _transactionService.GetLast12MonthBalances(null);
                return new ApiResponse("Last 12 Month Balances Returned.", res);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
        }
        [HttpGet]
        [Route("GetLast12MonthBalances/{userId}")]
        public async Task<ActionResult> GetLast12MonthBalances(string userId)
        {
            try
            {
                return new OkObjectResult(await _transactionService.GetLast12MonthBalances(userId));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        [HttpPost]
        [Route("Deposit")]
        public async Task<ApiResponse> Deposit(DepositRequest depositRequest)
        {
            var result = await _transactionService.DepositFunds(depositRequest);
            if (result == -1)
                return new ApiResponse($"no Account exists with accountId {depositRequest.AccountId}", 204);
            return new ApiResponse($"{depositRequest.Amount}$ Deposited Successfully");

        }
    }
}