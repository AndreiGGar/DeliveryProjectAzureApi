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
    public class DeliveryProjectController : ControllerBase
    {
        private RepositoryDelivery repo;

        public DeliveryProjectController(RepositoryDelivery repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Restaurant>>> Restaurants()
        {
            return await this.repo.GetRestaurantsAsync();
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Restaurant>> RestaurantById(int id)
        {
            return await this.repo.GetRestaurantByIdAsync(id);
        }

        [HttpGet]
        [Route("[action]/{search}")]
        public async Task<ActionResult<List<Restaurant>>> RestaurantsSearch(string search)
        {
            return await this.repo.GetRestaurantBySearchAsync(search);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Category>>> Categories()
        {
            return await this.repo.GetCategoriesAsync();
        }

        [HttpGet]
        [Route("[action]/{idcategory}")]
        public async Task<ActionResult<List<Restaurant>>> RestaurantsByCategory(int? idcategory = 0)
        {
            return await this.repo.GetRestaurantsByCategoryAsync(idcategory);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<CategoryProduct>>> RestaurantsCategories(int idrestaurant)
        {
            return await this.repo.GetRestaurantsCategoriesAsync(idrestaurant);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Product>>> RestaurantsCategoriesProducts(int idrestaurant)
        {
            return await this.repo.GetRestaurantsCategoriesProductsAsync(idrestaurant);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> InsertPurchaseProduct(InsertPurchaseProductModel model)
        {
            await this.repo.InsertPurchaseProductAsync(model.IdPurchase, model.IdProduct, model.Quantity);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> InsertPurchase(InsertPurchaseModel model)
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonEmpleado = claim.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonEmpleado);
            await this.repo.InsertPurchaseAsync(model.Id, user.Id, model.RestaurantId, model.TotalPrice, model.Status, model.Delivery, model.RequestDate, model.DeliveryAddress, model.DeliveryMethod, model.Code, model.Products, model.PaymentMethod);
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
    }
}
