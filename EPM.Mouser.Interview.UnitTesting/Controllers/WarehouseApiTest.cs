using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using EPM.Mouser.Interview.Models;
using EPM.Mouser.Interview.Web.Controllers;
using EPM.Mouser.Interview.Web.Interfaces.Services;
using Moq;

namespace EPM.Mouser.Interview.UnitTesting.Controllers
{
    public class WarehouseApiTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IWarehouseService> _warehouseServiceMock;
        private readonly WarehouseApi _sut;
        public WarehouseApiTest()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());

            _warehouseServiceMock = _fixture.Freeze<Mock<IWarehouseService>>();
            _sut = new WarehouseApi(_warehouseServiceMock.Object);
        }

        [Theory, AutoData]
        public async Task GetProduct_ShouldReturnOkResponse_WhenValidRequest(long id)
        {
            //Arrange
            var response = _fixture.Create<Product>();
            _warehouseServiceMock.Setup(x => x.GetProduct(id)).ReturnsAsync(response);

            //Act
            var result = await _sut.GetProduct(id);

            //Assert
            Assert.NotNull(response);
            response.Equals(result.Value);
        }

        [Fact]
        public async Task GetPublicInStockProducts_ShouldReturnOkResponse_WhenInValidRequest()
        {
            //Arrange
            var response = _fixture.Create<IEnumerable<Product>>();
            _warehouseServiceMock.Setup(x => x.GetPublicInStockProducts()).ReturnsAsync(response);

            //Act   
            var result = await _sut.GetPublicInStockProducts();

            //Assert
            Assert.NotNull(response);
            response.Equals(result.Value);
        }

        [Theory, AutoData]
        public async Task OrderItem_ShouldReturnOkResponse_WhenValidRequest(UpdateQuantityRequest updateQuantityRequest)
        {
            //Arrange
            var response = _fixture.Create<UpdateResponse>();
            _warehouseServiceMock.Setup(x => x.OrderItem(updateQuantityRequest)).ReturnsAsync(response);

            //Act
            var result = await _sut.OrderItem(updateQuantityRequest);

            //Assert
            Assert.NotNull(response);
            response.Equals(result.Value);
        }

        [Theory, AutoData]
        public async Task ShipItem_ShouldReturnOkResponse_WhenValidRequest(UpdateQuantityRequest updateQuantityRequest)
        {
            //Arrange
            var response = _fixture.Create<UpdateResponse>();
            _warehouseServiceMock.Setup(x => x.ShipItem(updateQuantityRequest)).ReturnsAsync(response);

            //Act
            var result = await _sut.ShipItem(updateQuantityRequest);

            //Assert
            Assert.NotNull(response);
            response.Equals(result.Value);
        }

        [Theory, AutoData]
        public async Task RestockItem_ShouldReturnOkResponse_WhenValidRequest(UpdateQuantityRequest updateQuantityRequest)
        {
            //Arrange
            var response = _fixture.Create<UpdateResponse>();
            _warehouseServiceMock.Setup(x => x.RestockItem(updateQuantityRequest)).ReturnsAsync(response);

            //Act
            var result = await _sut.RestockItem(updateQuantityRequest);

            //Assert
            Assert.NotNull(response);
            response.Equals(result.Value);
        }

        [Theory, AutoData]
        public async Task AddNewProduct_ShouldReturnOkResponse_WhenValidRequest(Product product)
        {
            //Arrange
            var response = _fixture.Create<CreateResponse<Product>>();
            _warehouseServiceMock.Setup(x => x.AddNewProduct(product)).ReturnsAsync(response);

            //Act
            var result = await _sut.AddNewProduct(product);

            //Assert
            Assert.NotNull(response);
            response.Equals(result.Value);
        }
    }
}
