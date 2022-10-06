using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectDK.Automapper;
using ProjectDK.BL.Services;
using ProjectDK.Controllers;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;
using ProjectDK.Models.Requests;
using ProjectDK.Models.Responses;

namespace ProjectDK.Test
{
    public class AuthorTests
    {
        private IList<Author> authors = new List<Author>()
        {
            new Author()
            {
                Id = 1,
                Age= 74,
                DateOfBirth = DateTime.Now,
                Name = "Author name",
                Nickname = "NickName"
            },
            new Author()
            {
                Id = 2,
                Age= 54,
                DateOfBirth = DateTime.Now,
                Name = "Another name",
                Nickname = "Another NickName"
            },
        };
        private readonly IMapper mapper;
        private Mock<ILogger<AuthorService>> loggerMock;
        private Mock<ILogger<AuthorController>> loggerControllerMock;
        private readonly Mock<IAuthorRepository> authorRepositoryMock;
        private readonly Mock<IBookRepository> bookRepositoryMock;
        public AuthorTests()
        {
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });
            mapper = mockMapperConfig.CreateMapper();
            loggerMock = new Mock<ILogger<AuthorService>>();
            loggerControllerMock = new Mock<ILogger<AuthorController>>();
            authorRepositoryMock = new Mock<IAuthorRepository>();
            bookRepositoryMock = new Mock<IBookRepository>();
        }

        [Fact]
        public async Task Author_GetAll_Count_Test()
        {
            var expectedCount = 2;
            authorRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(authors);

            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            var controller = new AuthorController(service, loggerControllerMock.Object, mapper);

            var result = await controller.GetAll();
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var authorsResult = okObjectResult.Value as IEnumerable<Author>;
            Assert.NotNull(authorsResult);
            Assert.NotEmpty(authorsResult);
            Assert.Equal(expectedCount, authorsResult.Count());
            Assert.Equal(authorsResult, authors);
        }

        [Fact]
        public async Task Author_GetAll_Count_Empty()
        {
            authorRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(()=>Enumerable.Empty<Author>());

            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            var controller = new AuthorController(service, loggerControllerMock.Object, mapper);

            var result = await controller.GetAll();
            var okObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(okObjectResult);

            var authorsResult = okObjectResult.Value as string;
            Assert.NotNull(authorsResult);
            Assert.Equal("There aren't any authors in the collection", authorsResult);
        }

        [Fact]

        public async Task Author_GetAuthorById_OK()
        {
            // setup
            var authorId = 1;
            var expectedAuthor = authors.First(x => x.Id == authorId);

            authorRepositoryMock.Setup(x => x.GetById(authorId)).ReturnsAsync(expectedAuthor);

            //inject
            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            var controller = new AuthorController(service, loggerControllerMock.Object, mapper);

            //act 
            var result = await controller.GetById(authorId);
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var author = okObjectResult.Value as Author;
            Assert.NotNull(author);
            Assert.Equal(expectedAuthor.Id, authorId);
        }

        [Fact]

        public async Task Author_GetAuthorById_NotFound()
        {
            // setup
            var authorId = 3;
            var expectedAuthor = authors.FirstOrDefault(x => x.Id == authorId);

            authorRepositoryMock.Setup(x => x.GetById(authorId)).ReturnsAsync(expectedAuthor);

            //inject
            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            var controller = new AuthorController(service, loggerControllerMock.Object, mapper);

            //act 
            var result = await controller.GetById(authorId);
            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundObjectResult);
            Assert.True(notFoundObjectResult.Value is int);
            Assert.Equal(authorId, (int)notFoundObjectResult.Value);
        }

        [Fact]

        public async Task Author_Add_OK()
        {
            //setup

            var authorRequest = new AuthorRequest()
            {               
                Nickname = "new nick",
                Age = 22,
                DateOfBirth = DateTime.Now,
                Name = "Test author name"
            };

            var expectedAuthorId = 3;
            authorRepositoryMock.Setup(x => x.Add(It.IsAny<Author>())).Callback(() =>
            {
                authors.Add(new Author()
                {
                    Id = expectedAuthorId,
                    Name = authorRequest.Name,
                    Age = authorRequest.Age,
                    DateOfBirth = authorRequest.DateOfBirth,
                    Nickname = authorRequest.Nickname
                });
            })!.ReturnsAsync(() => authors.FirstOrDefault(x => x.Id == expectedAuthorId));

            //inject
            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            var controller = new AuthorController(service, loggerControllerMock.Object, mapper);

            //act
            var result = await controller.Add(authorRequest);

            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult.Value as AddAuthorResponse;
            Assert.NotNull(resultValue);
            Assert.Equal(expectedAuthorId, resultValue.Author.Id);
        }

        [Fact]

        public async Task Author_AddAuthorWhenExist()
        {
            //setup

            var authorRequest = new AuthorRequest()
            {
                Nickname = "new nick",
                Age = 22,
                DateOfBirth = DateTime.Now,
                Name = "Author name"
            };

            authorRepositoryMock.Setup(x => x.GetByName(authorRequest.Name))
                .ReturnsAsync(() => authors.FirstOrDefault(x => x.Name == authorRequest.Name));

            //inject
            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            var controller = new AuthorController(service, loggerControllerMock.Object, mapper);

            //act
            var result = await controller.Add(authorRequest);

            //assert
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);

            var resultValue = badRequestObjectResult.Value as AuthorRequest;
            Assert.NotNull(resultValue);
            Assert.Equal(resultValue.Name, authorRequest.Name);
        }

        [Fact]

        public async Task Author_GetByName_NotNullTest()
        {
            // setup
            var authorName = "Author name";
            var expectedAuthor = authors.First(x => x.Name == authorName);

            authorRepositoryMock.Setup(x => x.GetByName(authorName)).ReturnsAsync(expectedAuthor);

            //inject
            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            //act 
            var result = await service.GetByName(authorName);
            var author = result as Author;
            Assert.NotNull(author);

            Assert.Equal(expectedAuthor.Id, author.Id);
        }

        [Fact]
        public async Task Author_GetByName_NullTest()
        {
            // setup
            var authorName = "Author test name";
            var expectedAuthor = authors.FirstOrDefault(x => x.Name == authorName);

            authorRepositoryMock.Setup(x => x.GetByName(authorName)).ReturnsAsync(expectedAuthor);

            //inject
            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            //act 
            var result = await service.GetByName(authorName);
            Assert.Null(result);
        }

        [Fact]
        public async Task Author_UpdateAuthor_OK()
        {
            var author = new AuthorRequest()
            {
                Id = 1,
                Nickname = "new nick",
                Age = 12,
                DateOfBirth = DateTime.Now,
                Name = "Author new name"
            };

            var authorToUpdate = authors.FirstOrDefault(x => x.Id == author.Id);

            authorRepositoryMock.Setup(x => x.GetById(author.Id))
                .ReturnsAsync(authorToUpdate);
            authorRepositoryMock.Setup(x => x.Update(It.IsAny<Author>())).Callback(() =>
            {
                authors.Remove(authorToUpdate);
                authors.Add(new Author()
                {
                    Id = 1,
                    Nickname = "new nick",
                    Age = 12,
                    DateOfBirth = DateTime.Now,
                    Name = "Author new name"
                });
            });

            //inject
            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            var controller = new AuthorController(service, loggerControllerMock.Object, mapper);

            //act
            var result = await controller.Update(author);

            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult.Value as UpdateAuthorResponse;
            Assert.NotNull(resultValue);
            Assert.Equal("Successfully updated author.", resultValue.Message);
        }

        [Fact]
        public async Task Author_UpdateAuthor_NotFound()
        {
            var author = new AuthorRequest()
            {
                Id = 3,
                Nickname = "new nick",
                Age = 12,
                DateOfBirth = DateTime.Now,
                Name = "Author new name"
            };

            var authorToUpdate = authors.FirstOrDefault(x => x.Id == author.Id);

            authorRepositoryMock.Setup(x => x.GetById(author.Id))
                .ReturnsAsync(authorToUpdate);

            //inject
            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            var controller = new AuthorController(service, loggerControllerMock.Object, mapper);

            //act
            var result = await controller.Update(author);

            //assert
            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundObjectResult);

            var resultValue = notFoundObjectResult.Value as UpdateAuthorResponse;
            Assert.NotNull(resultValue);
            Assert.Equal("Author to update does not exist", resultValue.Message);
        }

        [Fact]
        public async Task Author_UpdateAuthor_BadRequest()
        {
            //inject
            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            var controller = new AuthorController(service, loggerControllerMock.Object, mapper);

            //act
            var result = await controller.Update(null);

            //assert
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);

            var resultValue = badRequestObjectResult.Value as string;
            Assert.NotNull(resultValue);
            Assert.Equal("Author can't be null", resultValue);
        }

        [Fact]

        public async Task Author_Delete_OK()
        {
            var authorToDelete = authors.FirstOrDefault(x => x.Id == 1);

            authorRepositoryMock.Setup(x => x.GetById(authorToDelete.Id))
            .ReturnsAsync(authorToDelete);
            authorRepositoryMock.Setup(x => x.Delete(authorToDelete.Id)).ReturnsAsync(() => authorToDelete);

            //inject
            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            var controller = new AuthorController(service, loggerControllerMock.Object, mapper);

            //act
            var result = await controller.Delete(authorToDelete.Id);

            //assert

            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult.Value as Author;
            Assert.NotNull(resultValue);
            Assert.Equal(authorToDelete, resultValue);
        }

        [Fact]

        public async Task Author_Delete_NotFound()
        {
            var author = new Author()
            {
                Id = 3,
                Nickname = "new nick",
                Age = 12,
                DateOfBirth = DateTime.Now,
                Name = "Author new name"
            };
            var authorToDelete = authors.FirstOrDefault(x => x.Id == author.Id);

            authorRepositoryMock.Setup(x => x.Delete(author.Id)).ReturnsAsync(() => authorToDelete);

            //inject
            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            var controller = new AuthorController(service, loggerControllerMock.Object, mapper);

            //act
            var result = await controller.Delete(author.Id);
            var serviceResult = await service.Delete(author.Id);

            //assert

            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundObjectResult);

            var resultValue = (int) notFoundObjectResult.Value;
            Assert.NotNull(resultValue);
            Assert.Equal(author.Id, resultValue);
        }

        [Fact]

        public async Task Author_Delete_EmptyCollectionResult()
        {
            var author = new Author()
            {
                Id = 4,
                Nickname = "new nick",
                Age = 12,
                DateOfBirth = DateTime.Now,
                Name = "Author new name"
            };
            var authorToDelete = authors.FirstOrDefault(x => x.Id == author.Id);

            authorRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(() => Enumerable.Empty<Author>());
            authorRepositoryMock.Setup(x => x.Delete(author.Id)).ReturnsAsync(() => authorToDelete);

            //inject
            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            var controller = new AuthorController(service, loggerControllerMock.Object, mapper);

            //act
            var result = await controller.Delete(author.Id);

            //assert

            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundObjectResult);

            var resultValue = (int)notFoundObjectResult.Value;
            Assert.Equal(author.Id, resultValue);
        }

        [Fact]
        public async Task Author_Delete_InvalidId()
        {
            //setup
            var id = -1;
            //inject
            var service = new AuthorService(authorRepositoryMock.Object, mapper, bookRepositoryMock.Object, loggerMock.Object);

            var controller = new AuthorController(service, loggerControllerMock.Object, mapper);

            //act
            var result = await controller.Delete(id);

            //assert
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);

            var resultValue = badRequestObjectResult.Value as string;
            Assert.NotNull(resultValue);
            Assert.Equal("Id must be greater than 0", resultValue);
        }
    }
}