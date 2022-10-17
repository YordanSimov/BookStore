using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectDK.BL.Interfaces;
using ProjectDK.Models.MediatR.Commands;
using ProjectDK.Models.Models;

namespace ProjectDK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService shoppingCartService;
        private readonly IMediator mediator;
        private readonly IPurchaseService purchaseService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IMediator mediator, IPurchaseService purchaseService)
        {
            this.shoppingCartService = shoppingCartService;
            this.mediator = mediator;
            this.purchaseService = purchaseService;
        }

        [HttpGet(nameof(GetContent))]
        public async Task<IActionResult> GetContent([FromQuery] int userId)
        {
            return Ok(await shoppingCartService.GetContent(userId));
        }

        [HttpPost(nameof(AddToCart))]

        public async Task<IActionResult> AddToCart([FromQuery] int userId, [FromBody] Book book)
        {
            var bookCheck = await mediator.Send(new GetByIdBookCommand(book.Id));
            if (bookCheck != null)
            {
                await shoppingCartService.AddToCart(userId, bookCheck);
                return Ok();
            }
            return BadRequest("Book does not exist");
        }

        [HttpPost(nameof(Remove))]

        public async Task<IActionResult> Remove([FromBody]ShoppingCart shoppingCart)
        {
            var result = await shoppingCartService.RemoveFromCart(shoppingCart);
            if (result != null)
            {
                return Ok();
            }
            return BadRequest("Failed to remove book");
        }

        [HttpPost(nameof(Empty))]
        public async Task<IActionResult> Empty([FromQuery] int userId)
        {
            var result = await shoppingCartService.EmptyCart(userId);
            if (result)
            {
                return Ok("Successfully emptied collection");
            }
            return BadRequest();
        }
        [HttpGet(nameof(GetShoppingCart))]

        public async Task<IActionResult> GetShoppingCart(int userId)
        {
            var shoppingCart = await shoppingCartService.GetShoppingCart(userId);
            return Ok(shoppingCart);
        }

        [HttpPost(nameof(FinishPurchase))]
        public async Task<IActionResult> FinishPurchase([FromQuery] int userId)
        {
            await shoppingCartService.FinishPurchase(userId);
            return Ok();
        }

        [HttpGet(nameof(GetPurchases))]
        
        public async Task<IActionResult> GetPurchases(int userId)
        {
            var purchases= await purchaseService.GetPurchases(userId);
            return Ok(purchases);
        }

        [HttpPost(nameof(DeletePurchases))]

        public async Task<IActionResult> DeletePurchases([FromBody] Purchase purchase)
        {
            var deleted = await purchaseService.DeletePurchase(purchase);
            return Ok(deleted);
        }
    }
}
