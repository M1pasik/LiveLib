using LiveLib.Domain.Models;

namespace LiveLib.Database
{
    public static class DbInitializer
    {
        public static void Initialize(PostgresDatabaseContext context)
        {
            context.Database.EnsureCreated();

            // Проверяем, есть ли уже данные
            if (context.Books.Any()) return;

            // Создаем авторов
            var authors = new Author[]
            {
            new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "Лев",
                SecondName = "Толстой",
                ThirdName = "Николаевич",
                Biography = "Классик русской литературы, автор 'Войны и мира'"
            },
            new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "Фёдор",
                SecondName = "Достоевский",
                ThirdName = "Михайлович",
                Biography = "Автор 'Преступления и наказания'"
            },
            new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "Айзек",
                SecondName = "Азимов",
                ThirdName = "",
                Biography = "Мастер научной фантастики"
            }
            };
            context.Authors.AddRange(authors);

            // Создаем издателей
            var publishers = new BookPublisher[]
            {
            new BookPublisher
            {
                Id = Guid.NewGuid(),
                Name = "АСТ",
                Description = "Крупнейшее издательство России"
            },
            new BookPublisher
            {
                Id = Guid.NewGuid(),
                Name = "Эксмо",
                Description = "Ведущее издательство художественной литературы"
            }
            };
            context.BookPublishers.AddRange(publishers);

            // Создаем жанры
            var genres = new Genre[]
            {
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Классическая литература",
                AgeRestriction = 12
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Научная фантастика",
                AgeRestriction = 12
            },
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Детектив",
                AgeRestriction = 16
            }
            };
            context.Genres.AddRange(genres);

            // Сохраняем, чтобы получить ID
            context.SaveChanges();

            // Создаем книги
            var books = new Book[]
            {
            new Book
            {
                Id = Guid.NewGuid(),
                Name = "Война и мир",
                PublicatedAt = new DateOnly(1869, 1, 1),
                PageCount = 1225,
                Description = "Роман-эпопея о войне 1812 года",
                AverageRating = 4.9,
                Isbn = "978-5-17-090146-8",
                CoverId = "cover_war_peace",
                AuthorId = authors[0].Id,
                BookPublisherId = publishers[0].Id,
                GenreId = genres[0].Id
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Name = "Преступление и наказание",
                PublicatedAt = new DateOnly(1866, 1, 1),
                PageCount = 608,
                Description = "Психологический роман о преступлении",
                AverageRating = 4.8,
                Isbn = "978-5-04-099216-1",
                CoverId = "cover_crime",
                AuthorId = authors[1].Id,
                BookPublisherId = publishers[1].Id,
                GenreId = genres[0].Id
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Name = "Я, Робот",
                PublicatedAt = new DateOnly(1950, 1, 1),
                PageCount = 320,
                Description = "Сборник фантастических рассказов",
                AverageRating = 4.7,
                Isbn = "978-5-17-080115-7",
                CoverId = "cover_i_robot",
                AuthorId = authors[2].Id,
                BookPublisherId = publishers[0].Id,
                GenreId = genres[1].Id
            }
            };
            context.Books.AddRange(books);

            // Создаем пользователей (пароли в реальном проекте должны быть хешированы!)
            var users = new User[]
            {
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Иван Петров",
                Email = "ivan@example.com",
                PasswordHash = "dummy_hash_1",
                Role = "User"
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Мария Сидорова",
                Email = "maria@example.com",
                PasswordHash = "dummy_hash_2",
                Role = "User"
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Администратор",
                Email = "admin@example.com",
                PasswordHash = "admin_hash",
                Role = "Admin"
            }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            // Создаем коллекции
            var collections = new Collection[]
            {
            new Collection
            {
                Id = Guid.NewGuid(),
                Title = "Классика мировой литературы",
                OwnerUserId = users[0].Id,
                Books = new List<Book> { books[0], books[1] }
            },
            new Collection
            {
                Id = Guid.NewGuid(),
                Title = "Лучшая фантастика",
                OwnerUserId = users[1].Id,
                Books = new List<Book> { books[2] }
            }
            };

            // Настраиваем подписки пользователей на коллекции
            collections[0].UsersSubscribers = new List<User> { users[1] };
            collections[1].UsersSubscribers = new List<User> { users[0] };

            context.Collections.AddRange(collections);
            context.SaveChanges();

            // Создаем отзывы
            var reviews = new Review[]
            {
            new Review
            {
                Id = Guid.NewGuid(),
                UserId = users[0].Id,
                BookId = books[0].Id,
                Rate = 5.0,
                Comment = "Великое произведение!",
                IsRecommended = true
            },
            new Review
            {
                Id = Guid.NewGuid(),
                UserId = users[1].Id,
                BookId = books[2].Id,
                Rate = 4.5,
                Comment = "Интересный взгляд на будущее ИИ",
                IsRecommended = true
            }
            };
            context.Reviews.AddRange(reviews);

            context.SaveChanges();
        }
    }
}
