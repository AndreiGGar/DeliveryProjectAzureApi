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
    public class NoTokenController : ControllerBase
    {
        private RepositoryDelivery repo;

        public NoTokenController(RepositoryDelivery repo)
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
        [Route("[action]/{idrestaurant}")]
        public async Task<ActionResult<List<CategoryProduct>>> RestaurantsCategories(int idrestaurant)
        {
            return await this.repo.GetRestaurantsCategoriesAsync(idrestaurant);
        }

        [HttpGet]
        [Route("[action]/{idrestaurant}")]
        public async Task<ActionResult<List<Product>>> RestaurantsCategoriesProducts(int idrestaurant)
        {
            return await this.repo.GetRestaurantsCategoriesProductsAsync(idrestaurant);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Product>>> Products()
        {
            return await this.repo.GetProductsAsync();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> InsertPurchaseProduct(InsertPurchaseProductModel model)
        {
            await this.repo.InsertPurchaseProductAsync(model.IdPurchase, model.IdProduct, model.Quantity);
            return Ok();
        }
    }
}
