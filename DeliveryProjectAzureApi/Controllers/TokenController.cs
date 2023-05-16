using DeliveryProjectAzureApi.Repositories;
using DeliveryProjectNuget.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace DeliveryProjectAzureApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private RepositoryDelivery repo;

        public TokenController(RepositoryDelivery repo)
        {
            this.repo = repo;
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> InsertPurchase(InsertPurchaseModel model)
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonEmpleado = claim.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonEmpleado);
            await this.repo.InsertPurchaseAsync(user.Id, model.RestaurantId, model.TotalPrice, model.Status, model.Delivery, DateTime.Parse(model.RequestDate), model.DeliveryAddress, model.DeliveryMethod, model.Code, model.Products, model.PaymentMethod);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Restaurant>>> RestaurantsWishlist()
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonEmpleado = claim.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonEmpleado);
            List<Restaurant> restaurants = await this.repo.GetRestaurantsWithWishlistAsync(user.Id);
            return restaurants;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]/{idrestaurant}")]
        public async Task<ActionResult<bool>> RestaurantsInWishlist(int idrestaurant)
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonEmpleado = claim.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonEmpleado);
            return await this.repo.RestaurantExistsInWishlist(user.Id, idrestaurant);
        }

        [Authorize]
        [HttpPost]
        [Route("[action]/{idrestaurant}/{dateAdd}")]
        public async Task<ActionResult> AddToWishlist(int idrestaurant, string dateAdd)
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonEmpleado = claim.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonEmpleado);
            await this.repo.AddToWishlist(user.Id, idrestaurant, dateAdd);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("[action]/{idrestaurant}")]
        public async Task<ActionResult<Wishlist>> GetWishlistItem(int idrestaurant)
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonEmpleado = claim.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonEmpleado);
            Wishlist wishlist = await this.repo.GetWishlistItem(user.Id, idrestaurant);
            return wishlist;
        }

        [Authorize]
        [HttpPost]
        [Route("[action]/{idrestaurant}")]
        public async Task<ActionResult> DeleteFromWishlist(int idrestaurant)
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonEmpleado = claim.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonEmpleado);
            await this.repo.DeleteFromWishlist(user.Id, idrestaurant);
            return Ok();
        }


        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<User>> UserProfile()
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonEmpleado = claim.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonEmpleado);
            return user;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Purchase>>> PurchasesByUser()
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonEmpleado = claim.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonEmpleado);
            List<Purchase> purchases = await this.repo.GetPurchasesByUserIdAsync(user.Id);
            return purchases;
        }
    }
}
