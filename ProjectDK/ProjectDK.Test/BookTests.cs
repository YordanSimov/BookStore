using AutoMapper;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDK.Test
{
    public class BookTests
    {
        private IList<Book> books = new List<Book>()
        {
            new Book()
            {
                Id = 1,
                Title = "test",
                AuthorId = 1,
                LastUpdated = DateTime.Now,
                Price = 100,
                Quantity = 10,
            },
              new Book()
            {
                Id = 2,
                Title = "test2",
                AuthorId = 2,
                LastUpdated = DateTime.Now,
                Price = 120,
                Quantity = 13,
            }
        };
        private readonly IMapper mapper;
        private Mock<ILogger<BookService>> loggerMock;
        private Mock<ILogger<AuthorController>> loggerControllerMock;
        private readonly Mock<IAuthorRepository> authorRepositoryMock;
        private readonly Mock<IBookRepository> bookRepositoryMock;
        public BookTests()
        {
            var mockMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });
            mapper = mockMapperConfig.CreateMapper();
            loggerMock = new Mock<ILogger<BookService>>();
            loggerControllerMock = new Mock<ILogger<AuthorController>>();
            authorRepositoryMock = new Mock<IAuthorRepository>();
            bookRepositoryMock = new Mock<IBookRepository>();
        }
        [Fact]
        public async Task Book_GetAll_Count_Test()
        {
            var expectedCount = 2;
            bookRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(books);

            var service = new BookService(bookRepositoryMock.Object, mapper, authorRepositoryMock.Object);

            var controller = new BookController(service, loggerControllerMock.Object);

            var result = await controller.GetAll();
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var booksResult = okObjectResult.Value as IEnumerable<Book>;
            Assert.NotNull(booksResult);
            Assert.NotEmpty(booksResult);
            Assert.Equal(expectedCount, booksResult.Count());
            Assert.Equal(booksResult, books);
        }

        [Fact]
        public async Task Author_GetAll_Count_Empty()
        {
            bookRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(() => Enumerable.Empty<Book>());

            var service = new BookService(bookRepositoryMock.Object, mapper, authorRepositoryMock.Object);

            var controller = new BookController(service, loggerControllerMock.Object);

            var result = await controller.GetAll();
            var okObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(okObjectResult);

            var messageResult = okObjectResult.Value as string;
            Assert.NotNull(messageResult);
            Assert.Equal("There aren't any books in the collection", messageResult);
        }

        [Fact]

        public async Task Book_GetBookById_OK()
        {
            // setup
            var bookId = 1;
            var expectedBook = books.First(x => x.Id == bookId);

            bookRepositoryMock.Setup(x => x.GetById(bookId)).ReturnsAsync(expectedBook);

            //inject
            var service = new BookService(bookRepositoryMock.Object, mapper, authorRepositoryMock.Object);
            var controller = new BookController(service, loggerControllerMock.Object);


            //act 
            var result = await controller.GetById(bookId);
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var book = okObjectResult.Value as Book;
            Assert.NotNull(book);
            Assert.Equal(expectedBook.Id, bookId);
        }

        [Fact]

        public async Task Book_GetBookById_NotFound()
        {
            // setup
            var bookId = 3;
            var expectedBook = books.FirstOrDefault(x => x.Id == bookId);

            bookRepositoryMock.Setup(x => x.GetById(bookId)).ReturnsAsync(expectedBook);

            //inject
            var service = new BookService(bookRepositoryMock.Object, mapper, authorRepositoryMock.Object);
            var controller = new BookController(service, loggerControllerMock.Object);


            //act 
            var result = await controller.GetById(bookId);

            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundObjectResult);
            Assert.True(notFoundObjectResult.Value is int);
            Assert.Equal(bookId, (int)notFoundObjectResult.Value);
        }

        [Fact]

        public async Task Book_Add_OK()
        {
            //setup

            var bookRequest = new BookRequest()
            {
                Title = "test add",
                AuthorId = 1,
                LastUpdated = DateTime.Now,
                Price = 110,
                Quantity = 40,
            };

            var authors = new List<Author>()
            {
                new Author()
                {
                    Id = 1,
                    Age= 74,
                    DateOfBirth = DateTime.Now,
                    Name = "Author name",
                    Nickname = "NickName"
                }
            };

            var expectebookId = 3;
            bookRepositoryMock.Setup(x => x.GetById(expectebookId))
                .ReturnsAsync(books.FirstOrDefault(x => x.Id == expectebookId));
            authorRepositoryMock.Setup(x => x.GetById(bookRequest.AuthorId))
                .ReturnsAsync(() => authors.FirstOrDefault(x => x.Id == bookRequest.AuthorId));
            bookRepositoryMock.Setup(x => x.Add(It.IsAny<Book>())).Callback(() =>
            {
                books.Add(new Book()
                {
                    Id = expectebookId,
                    Title = bookRequest.Title,
                    LastUpdated = bookRequest.LastUpdated,
                    Price = bookRequest.Price,
                    Quantity = bookRequest.Quantity,
                    AuthorId = bookRequest.AuthorId
                });
            })!.ReturnsAsync(() => books.FirstOrDefault(x => x.Id == expectebookId));

            //inject
            var service = new BookService(bookRepositoryMock.Object, mapper, authorRepositoryMock.Object);
            var controller = new BookController(service, loggerControllerMock.Object);

            //act
            var result = await controller.Add(bookRequest);

            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult.Value as BookResponse;
            Assert.NotNull(resultValue);
            Assert.Equal(expectebookId, resultValue.Book.Id);
        }

        [Fact]

        public async Task Book_Add_WhenAuthorExists()
        {
            //setup

            var bookRequest = new BookRequest()
            {
                Title = "test add",
                AuthorId = 1,
                LastUpdated = DateTime.Now,
                Price = 110,
                Quantity = 40,
            };


            var authors = new List<Author>()
            {
                new Author()
                {
                    Id = 3,
                    Age= 74,
                    DateOfBirth = DateTime.Now,
                    Name = "Author name",
                    Nickname = "NickName"
                }
            };

            var expectedBookId = 3;

            bookRepositoryMock.Setup(x => x.GetById(bookRequest.Id))
                .ReturnsAsync(books.FirstOrDefault(x => x.Id == expectedBookId));
            authorRepositoryMock.Setup(x => x.GetById(bookRequest.AuthorId))
              .ReturnsAsync(() => authors.FirstOrDefault(x => x.Id == bookRequest.AuthorId));
            bookRepositoryMock.Setup(x => x.Add(It.IsAny<Book>())).Callback(() =>
            {
                books.Add(new Book()
                {
                    Id = expectedBookId,
                    Title = bookRequest.Title,
                    LastUpdated = bookRequest.LastUpdated,
                    Price = bookRequest.Price,
                    Quantity = bookRequest.Quantity,
                    AuthorId = bookRequest.AuthorId
                });
            })!.ReturnsAsync(() => books.FirstOrDefault(x => x.Id == expectedBookId));

            //inject
            var service = new BookService(bookRepositoryMock.Object, mapper, authorRepositoryMock.Object);
            var controller = new BookController(service, loggerControllerMock.Object);

            //act
            var result = await controller.Add(bookRequest);

            //assert
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);

            var resultValue = badRequestObjectResult.Value as BookResponse;
            Assert.NotNull(resultValue);
            Assert.Equal("Author does not exist", resultValue.Message);
        }

        [Fact]

        public async Task Book_Add_WhenBookExists()
        {
            //setup

            var bookRequest = new BookRequest()
            {
                Id = 1,
                Title = "test add",
                AuthorId = 1,
                LastUpdated = DateTime.Now,
                Price = 110,
                Quantity = 40,
            };

            bookRepositoryMock.Setup(x => x.GetById(bookRequest.Id))
                .ReturnsAsync(books.FirstOrDefault(x => x.Id == bookRequest.Id));
            bookRepositoryMock.Setup(x => x.Add(It.IsAny<Book>())).Callback(() =>
            {
                books.Add(new Book()
                {
                    Id = bookRequest.Id,
                    Title = bookRequest.Title,
                    LastUpdated = bookRequest.LastUpdated,
                    Price = bookRequest.Price,
                    Quantity = bookRequest.Quantity,
                    AuthorId = bookRequest.AuthorId
                });
            })!.ReturnsAsync(() => books.FirstOrDefault(x => x.Id == bookRequest.Id));

            //inject
            var service = new BookService(bookRepositoryMock.Object, mapper, authorRepositoryMock.Object);
            var controller = new BookController(service, loggerControllerMock.Object);

            //act
            var result = await controller.Add(bookRequest);

            //assert
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);

            var resultValue = badRequestObjectResult.Value as BookResponse;
            Assert.NotNull(resultValue);
            Assert.Equal("Book already exists", resultValue.Message);
        }

        [Fact]
        public async Task Book_UpdateBook_OK()
        {
            //setup

            var bookRequest = new BookRequest()
            {
                Id = 1,
                Title = "test add",
                AuthorId = 1,
                LastUpdated = DateTime.Now,
                Price = 110,
                Quantity = 40,
            };

            var bookToUpdate = books.FirstOrDefault(x => x.Id == bookRequest.Id);

            bookRepositoryMock.Setup(x => x.GetById(bookRequest.Id))
                .ReturnsAsync(bookToUpdate);
            bookRepositoryMock.Setup(x => x.Update(It.IsAny<Book>())).Callback(() =>
            {
                books.Remove(bookToUpdate);
                books.Add(new Book()
                {
                    Id = bookRequest.Id,
                    Title = bookRequest.Title,
                    LastUpdated = bookRequest.LastUpdated,
                    Price = bookRequest.Price,
                    Quantity = bookRequest.Quantity,
                    AuthorId = bookRequest.AuthorId
                });
            });

            //inject
            var service = new BookService(bookRepositoryMock.Object, mapper, authorRepositoryMock.Object);
            var controller = new BookController(service, loggerControllerMock.Object);

            //act
            var result = await controller.Update(bookRequest);

            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult.Value as BookResponse;
            Assert.NotNull(resultValue);
            Assert.Equal(bookToUpdate.Id, resultValue.Book.Id);
        }

        [Fact]
        public async Task Book_UpdateBook_NotFound()
        {
            //setup

            var bookRequest = new BookRequest()
            {
                Id = 3,
                Title = "test add",
                AuthorId = 1,
                LastUpdated = DateTime.Now,
                Price = 110,
                Quantity = 40,
            };

            var bookToUpdate = books.FirstOrDefault(x => x.Id == bookRequest.Id);

            bookRepositoryMock.Setup(x => x.GetById(bookRequest.Id))
                .ReturnsAsync(bookToUpdate);

            //inject
            var service = new BookService(bookRepositoryMock.Object, mapper, authorRepositoryMock.Object);
            var controller = new BookController(service, loggerControllerMock.Object);

            //act
            var result = await controller.Update(bookRequest);

            //assert
            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundObjectResult);

            var resultValue = notFoundObjectResult.Value as BookResponse;
            Assert.NotNull(resultValue);
            Assert.Equal("Book does not exist.", resultValue.Message);
        }

        [Fact]
        public async Task Book_UpdateBook_BadRequest()
        {
            //inject
            var service = new BookService(bookRepositoryMock.Object, mapper, authorRepositoryMock.Object);
            var controller = new BookController(service, loggerControllerMock.Object);

            //act
            var result = await controller.Update(null);

            //assert
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestObjectResult);

            var resultValue = badRequestObjectResult.Value as string;
            Assert.NotNull(resultValue);
            Assert.Equal("Book can't be null", resultValue);
        }

        [Fact]

        public async Task Book_Delete_OK()
        {
            //setup

            var bookToDelete = books.FirstOrDefault(x => x.Id == 1);

            bookRepositoryMock.Setup(x => x.GetById(bookToDelete.Id))
            .ReturnsAsync(bookToDelete);
            bookRepositoryMock.Setup(x => x.Delete(bookToDelete.Id)).ReturnsAsync(() => bookToDelete);

            //inject
            var service = new BookService(bookRepositoryMock.Object, mapper, authorRepositoryMock.Object);
            var controller = new BookController(service, loggerControllerMock.Object);

            //act
            var result = await controller.Delete(bookToDelete.Id);

            //assert

            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var resultValue = okObjectResult.Value as Book;
            Assert.NotNull(resultValue);
            Assert.Equal(bookToDelete, resultValue);
        }
        [Fact]

        public async Task Book_Delete_NotFound()
        {
            //setup

            var book = new Book()
            {
                Id = 3,
                Title = "test add",
                AuthorId = 1,
                LastUpdated = DateTime.Now,
                Price = 110,
                Quantity = 40,
            };

            var bookToDelete = books.FirstOrDefault(x => x.Id == book.Id);

            bookRepositoryMock.Setup(x => x.GetById(book.Id))
            .ReturnsAsync(bookToDelete);
            bookRepositoryMock.Setup(x => x.Delete(book.Id)).ReturnsAsync(() => bookToDelete);

            //inject
            var service = new BookService(bookRepositoryMock.Object, mapper, authorRepositoryMock.Object);
            var controller = new BookController(service, loggerControllerMock.Object);

            //act
            var result = await controller.Delete(book.Id);

            //assert

            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundObjectResult);

            var resultValue = (int)notFoundObjectResult.Value;
            Assert.NotNull(resultValue);
            Assert.Equal(book.Id, resultValue);
        }

        [Fact]

        public async Task Book_Delete_InvalidId()
        {
            //setup
            var id = -1;
            //inject
            var service = new BookService(bookRepositoryMock.Object, mapper, authorRepositoryMock.Object);
            var controller = new BookController(service, loggerControllerMock.Object);

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
