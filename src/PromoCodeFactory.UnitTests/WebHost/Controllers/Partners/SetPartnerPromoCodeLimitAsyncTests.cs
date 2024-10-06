using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly PartnersController _partnersController;
        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _partnersRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();
            _partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
        }
        public Partner CreateBasePartner()
        {
            var partner = new Partner()
            {
                Id = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319"),
                Name = "Cats",
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
                {
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("c9bef066-3c5a-4e5d-9cff-bd54479f075e"),
                        CreateDate = new DateTime(2020,05,3),
                        EndDate = new DateTime(2020,10,15),
                        CancelDate = new DateTime(2020,06,16),
                        Limit = 1000
                    },
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("0e94624b-1ff9-430e-ba8d-ef1e3b77f2d5"),
                        CreateDate = new DateTime(2020,05,3),
                        EndDate = new DateTime(2020,10,15),
                        Limit = 100
                    },
                }
            };

            return partner;
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsNotFound_ReturnsNotFound()
        {
            // Arrenge
            var partnerId = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.UtcNow,
                Limit = 100
            };
            Partner partner = null;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert 
            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsNotActive_ReturnsBadRequest()
        {
            // Arrenge 
            var partnerId = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.UtcNow,
                Limit = 100
            };
            var partner = CreateBasePartner();
            partner.IsActive = false;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_SetLimit_ResetNumberOfPromoCodes()
        {
            // Arrenge
            var partnerId = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.UtcNow.AddYears(-100),
                Limit = 100
            };
            var partner = CreateBasePartner();
            partner.NumberIssuedPromoCodes = 50;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync(partner);
            var activeLimit = partner.PartnerLimits.FirstOrDefault(x =>
                !x.CancelDate.HasValue);
            // Act 
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert

            result.Should().BeOfType<CreatedAtActionResult>();
            if (activeLimit != null)
            {
                partner.NumberIssuedPromoCodes.Should().Be(0);
            }
            else
            {
                partner.NumberIssuedPromoCodes.Should().Be(50);
            }

            _partnersRepositoryMock.Verify(repo => repo.UpdateAsync(partner), Times.Once);
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_SetLimit_DisablesPreviousLimitAndResetsNumberOfPromoCodes()
        {
            // Arrange
            var partnerId = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.UtcNow.AddMonths(1),
                Limit = 200
            };
            var partner = CreateBasePartner();
            partner.NumberIssuedPromoCodes = 50;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            var activeLimit = partner.PartnerLimits.FirstOrDefault(x => !x.CancelDate.HasValue);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();

            if (activeLimit != null)
            {
                activeLimit.CancelDate.Should().NotBeNull(); // Предыдущий лимит должен быть отключен
                partner.NumberIssuedPromoCodes.Should().Be(0); // Количество выданных промокодов сбрасывается
            }
            else
            {
                partner.NumberIssuedPromoCodes.Should().Be(50);
            }

            _partnersRepositoryMock.Verify(repo => repo.UpdateAsync(partner), Times.Once);
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_LimitIsNotNull_ReturnsBadRequest()
        {
            // Arrenge
            var partnerId = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.UtcNow.AddMonths(1),
                Limit = -1
            };
            var partner = CreateBasePartner();

            // Act
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            // Assert
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_UpdateAsync_ShouldVerifyupdateAsync()
        {
            // Arrange
            var partnerId = Guid.Parse("894b6e9b-eb5f-406c-aefa-8ccb35d39319");
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.UtcNow.AddMonths(1),
                Limit = 200
            };
            var partner = CreateBasePartner();

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync(partner);


            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            _partnersRepositoryMock.Verify(repo => repo.UpdateAsync(partner), Times.Once);
        }
    }
}