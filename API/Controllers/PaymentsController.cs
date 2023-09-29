using System.Runtime.CompilerServices;
using API.Errors;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController: BaseAPIController
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger<PaymentsController> logger;
        private const string WhSecret = "whsec_fb0a5eabcde17e7d01075070d9d0905951f7c74a078da155ac0c5d97f1206aca";

        public PaymentsController(IPaymentService iPaymentService, ILogger<PaymentsController> ilogger)
        {
            this.paymentService = iPaymentService;
            this.logger = ilogger;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await paymentService.CreateOrUpdatePaymentIntent(basketId);

            if(basket == null)
            {
                return BadRequest(new ApiResponse(400, "Problem with your basket"));
            }

            return Ok(basket);
        }
        
        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebHook(){
            //Lê o body do request como json
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var headers = new Dictionary<string, StringValues>();
            foreach (var header in Request.Headers)
            {
                headers.Add(header.Key, header.Value);
            }

            var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();
            //O stripe vai passar o WhSecret como request header
           var stripeEvent = EventUtility.ConstructEvent(json, signature, WhSecret, tolerance: 300, throwOnApiVersionMismatch: false);
            PaymentIntent intent;
            Order order;
            switch(stripeEvent.Type)
            {
                case "payment_intent.succeeded" :
                    intent = (PaymentIntent) stripeEvent.Data.Object;
                    logger.LogInformation("Payment succeeded: ", intent.Id);
                    //Update the order with the new status
                    order = await paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                    logger.LogInformation("Update to payment received: ", order.Id);
                    break;
                
                case "payment_intent.payment_failed" :
                    intent = (PaymentIntent) stripeEvent.Data.Object;
                    logger.LogInformation("Payment failed: ", intent.Id);
                    //Update the order with the new status
                    order = await paymentService.UpdateOrderPaymentFailed(intent.Id);
                    logger.LogInformation("Update to payment not received: ", order.Id);
                    break;
            }

            return new EmptyResult();
        }

    }
}