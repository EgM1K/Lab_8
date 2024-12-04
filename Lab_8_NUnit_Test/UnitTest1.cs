using Microsoft.EntityFrameworkCore;
using RozetkaWedAPI.Controllers;
using RozetkaWedAPI.Data;
using RozetkaWedAPI.Models;
using RozetkaWedAPI.Servises.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RozetkaWebAPI.Services;
using NUnit.Framework.Legacy;
using Moq;

namespace Lab_8_NUnit_Test
{

    [TestFixture]
    public class CategoriesControllerTests
    {
        private StoreContext _context;
        private ICategoryService _categoryService;
        private CategoriesController _categoriesController;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new StoreContext(options);
            _context.Categories.AddRange(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" }
            );
            _context.SaveChanges();
            _categoryService = new CategoryService(_context);
            _categoriesController = new CategoriesController(_categoryService);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetCategories_ReturnsOkResult_WithListOfCategories()
        {
            var result = await _categoriesController.GetCategories();
            var okResult = result.Result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            var categories = okResult.Value as IEnumerable<Category>;
            ClassicAssert.IsNotNull(categories);
            ClassicAssert.AreEqual(2, categories.Count());
            var categoryList = categories.ToList();
            ClassicAssert.AreEqual("Electronics", categoryList[0].Name);
            ClassicAssert.AreEqual("Clothing", categoryList[1].Name);
        }

        [Test]
        public async Task GetCategory_WithValidId_ReturnsOkResult()
        {
            var result = await _categoriesController.GetCategory(1);
            var okResult = result.Result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            var category = okResult.Value as Category;
            ClassicAssert.IsNotNull(category);
            ClassicAssert.AreEqual("Electronics", category.Name);
        }

        [Test]
        public async Task GetCategory_WithInvalidId_ReturnsNotFound()
        {
            var result = await _categoriesController.GetCategory(99);
            ClassicAssert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task CreateCategory_AddsCategoryAndReturnsCreatedAtActionResult()
        {
            var newCategory = new Category { Name = "Home Appliances" };
            var result = await _categoriesController.CreateCategory(newCategory);
            var createdResult = result.Result as CreatedAtActionResult;
            ClassicAssert.IsNotNull(createdResult);
            ClassicAssert.AreEqual(201, createdResult.StatusCode);
            var createdCategory = createdResult.Value as Category;
            ClassicAssert.IsNotNull(createdCategory);
            ClassicAssert.AreEqual("Home Appliances", createdCategory.Name);
        }
    }

    [TestFixture]
    public class CategoryServiceTests
    {
        private StoreContext _dbContext;
        private CategoryService _categoryService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new StoreContext(options);
            _categoryService = new CategoryService(_dbContext);

            _dbContext.Categories.AddRange(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" }
            );
            _dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            var result = await _categoryService.GetAllAsync();
            ClassicAssert.IsNotNull(result);
            var resultList = result.ToList();
            ClassicAssert.AreEqual(2, resultList.Count);
            ClassicAssert.AreEqual("Electronics", resultList[0].Name);
            ClassicAssert.AreEqual("Clothing", resultList[1].Name);
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ReturnsCategory()
        {
            var result = await _categoryService.GetByIdAsync(1);
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("Electronics", result.Name);
        }

        [Test]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            var result = await _categoryService.GetByIdAsync(99);
            ClassicAssert.IsNull(result);
        }

        [Test]
        public async Task AddAsync_AddsCategoryToDatabase()
        {
            var newCategory = new Category { Name = "Books" };
            var result = await _categoryService.AddAsync(newCategory);
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("Books", result.Name);
            var allCategories = await _categoryService.GetAllAsync();
            ClassicAssert.AreEqual(3, allCategories.Count());
        }
    }

[TestFixture]
    public class ProductsControllerTests
    {
        private Mock<IProductService> _productServiceMock;
        private ProductsController _controller;
        [SetUp]
        public void SetUp()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductsController(_productServiceMock.Object);
        }

        [Test]
        public async Task GetProducts_ReturnsAllProducts()
        {
            var mockProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1000.00m },
            new Product { Id = 2, Name = "Smartphone", Price = 500.00m }
        };

            _productServiceMock.Setup(service => service.GetAllAsync())
                .ReturnsAsync(mockProducts);
            var result = await _controller.GetProducts();
            var okResult = result.Result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            var products = okResult.Value as IEnumerable<Product>;
            ClassicAssert.IsNotNull(products);
            ClassicAssert.AreEqual(2, mockProducts.Count);
        }

        [Test]
        public async Task GetProduct_WithValidId_ReturnsProduct()
        {
            var mockProduct = new Product { Id = 1, Name = "Laptop", Price = 1000.00m };
            _productServiceMock.Setup(service => service.GetByIdAsync(1))
                .ReturnsAsync(mockProduct);
            var result = await _controller.GetProduct(1);
            var okResult = result.Result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            var product = okResult.Value as Product;
            ClassicAssert.IsNotNull(product);
            ClassicAssert.AreEqual("Laptop", product.Name);
            ClassicAssert.AreEqual(1000.00m, product.Price);
        }

        [Test]
        public async Task GetProduct_WithInvalidId_ReturnsNotFound()
        {
            _productServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Product)null);
            var result = await _controller.GetProduct(99);
            ClassicAssert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task CreateProduct_ReturnsCreatedProduct()
        {
            var newProduct = new Product { Name = "Tablet", Price = 300.00m };
            var createdProduct = new Product { Id = 1, Name = "Tablet", Price = 300.00m };
            _productServiceMock.Setup(service => service.CreateAsync(newProduct))
                .ReturnsAsync(createdProduct);
            var result = await _controller.CreateProduct(newProduct);
            var createdResult = result.Result as CreatedAtActionResult;
            ClassicAssert.IsNotNull(createdResult);
            ClassicAssert.AreEqual(201, createdResult.StatusCode);
            var product = createdResult.Value as Product;
            ClassicAssert.IsNotNull(product);
            ClassicAssert.AreEqual("Tablet", product.Name);
            ClassicAssert.AreEqual(300.00m, product.Price);
        }

        [Test]
        public async Task UpdateProduct_WithValidId_ReturnsNoContent()
        {
            var updatedProduct = new Product { Name = "Updated Laptop", Price = 1200.00m };
            _productServiceMock.Setup(service => service.UpdateAsync(1, updatedProduct))
                .ReturnsAsync(true);
            var result = await _controller.UpdateProduct(1, updatedProduct);
            ClassicAssert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task UpdateProduct_WithInvalidId_ReturnsNotFound()
        {
            var updatedProduct = new Product { Name = "Updated Laptop", Price = 1200.00m };
            _productServiceMock.Setup(service => service.UpdateAsync(99, updatedProduct))
                .ReturnsAsync(false);
            var result = await _controller.UpdateProduct(99, updatedProduct);
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteProduct_WithValidId_ReturnsNoContent()
        {
            _productServiceMock.Setup(service => service.DeleteAsync(1))
                .ReturnsAsync(true);
            var result = await _controller.DeleteProduct(1);
            ClassicAssert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteProduct_WithInvalidId_ReturnsNotFound()
        {
            _productServiceMock.Setup(service => service.DeleteAsync(99))
                .ReturnsAsync(false);
            var result = await _controller.DeleteProduct(99);
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }
    }


    [TestFixture]
    public class OrdersControllerTests
    {
        private StoreContext _context;
        private IOrderService _orderService;
        private OrderController _ordersController;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Orders")
                .Options;
            _context = new StoreContext(options);
            _context.Orders.AddRange(
                new Order
                {
                    Id = 1,
                    BuyerName = "John Doe",
                    BuyerEmail = "john.doe@example.com",
                    OrderDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItem>
                    {
                    new OrderItem { Id = 1, ProductId = 1, Quantity = 2, UnitPrice = 50.00m }
                    }
                },
                new Order
                {
                    Id = 2,
                    BuyerName = "Jane Smith",
                    BuyerEmail = "jane.smith@example.com",
                    OrderDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItem>
                    {
                    new OrderItem { Id = 2, ProductId = 2, Quantity = 1, UnitPrice = 100.00m }
                    }
                }
            );
            _context.SaveChanges();
            _orderService = new OrderService(_context);
            _ordersController = new OrderController(_orderService);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAll_ReturnsAllOrders()
        {
            var result = await _ordersController.GetAll();
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            var orders = okResult.Value as IEnumerable<Order>;
            ClassicAssert.IsNotNull(orders);
            ClassicAssert.AreEqual(2, orders.Count());
        }

        [Test]
        public async Task GetById_ExistingId_ReturnsCorrectOrder()
        {
            var result = await _ordersController.GetById(1);
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            var order = okResult.Value as Order;
            ClassicAssert.IsNotNull(order);
            ClassicAssert.AreEqual("John Doe", order.BuyerName);
            ClassicAssert.AreEqual("john.doe@example.com", order.BuyerEmail);
        }
    }
}