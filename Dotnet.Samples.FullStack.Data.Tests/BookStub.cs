using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dotnet.Samples.FullStack.Data.Tests
{
    public static class BookStub
    {
        private static Random random = new Random();

        public static Book CreateNew()
        {
            return Builder<Book>
                .CreateNew()
                    .With(book => book.Isbn = CreateRandomFakeIsbn())
                    .With(book => book.Published = CreateRandomPastDate())
                    .With(book => book.Pages = random.Next(0, 1000))
                    .With(book => book.InStock = Convert.ToBoolean(random.Next(0, 2)))
                .Build();
        }

        public static List<Book> CreateListOfSize(int quantity)
        {
            if (quantity < 1)
            {
                throw new ArgumentOutOfRangeException("Quantity should be positive.");
            }

            return Builder<Book>
                .CreateListOfSize(quantity)
                    .All()
                        .With(book => book.Isbn = CreateRandomFakeIsbn())
                        .With(book => book.Published = CreateRandomPastDate())
                        .With(book => book.Pages = random.Next(0, 1000))
                        .With(book => book.InStock = Convert.ToBoolean(random.Next(0, 2)))
                .Build()
                    .ToList();
        }

        private static string CreateRandomFakeIsbn()
        {
            var ean = "978";
            var group = random.Next(0, 2).ToString("0");
            var publisher = random.Next(200, 699).ToString("000");
            var title = random.Next(0, 99999).ToString("00000");
            var check = random.Next(0, 10).ToString("0"); // Not a real checksum!

            return string.Format("{0}-{1}-{2}-{3}-{4}", ean, group, publisher, title, check);
        }

        private static DateTime CreateRandomPastDate()
        {
            var start = new DateTime(1900, 1, 1);
            var range = ((TimeSpan)(DateTime.Today - start)).Days;

            return start.AddDays(random.Next(range));
        }
    }
}
