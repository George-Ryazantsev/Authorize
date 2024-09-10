using BookLib_Auth_Mapp_NF.Data;
using BookLib_Auth_Mapp_NF.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using static System.Reflection.Metadata.BlobBuilder;


namespace BookLib_Auth_Mapp_NF.Services
{
    public class BooksUpdateService
    {
        private readonly BookContext _bookContext;
        public BooksUpdateService(BookContext bookContext)
        {
                _bookContext = bookContext;
        }
        public async Task UpdateBooksASync(string filepath)
        {            
            var lines = await File.ReadAllLinesAsync(filepath);
            var currentBooks = _bookContext.Books.ToList();
            var newBooks = new List<BookModel>();
            foreach (var line in lines) 
            {
                var title = line.Split('-')[0];
                var author = line.Split('-')[0];
                newBooks.Add(new BookModel { Title = title, Author = author });
                _bookContext.Books.Add(new BookModel{ Title = title, Author = author });// аналогично foreach внизу
            }
            var addedBooks = newBooks.Except(currentBooks, new BooksComparer()).ToList();
            // возвращает элементы в newBooks, которых нет в currentBooks
            var removedBooks = currentBooks.Except(newBooks, new BooksComparer()).ToList();
            // возвращает элементы в currentBooks, которых нет в newBooks
            foreach (var book in addedBooks) 
            {
                //_bookContext.Books.Add(book); // добавили новые BookModel'ки в контекст
                _bookContext.BookChanges.Add(new BookChanges
                { Title=book.Title, Author=book.Author, 
                    ChangeDate=DateTime.Now, ChangeType="Added"});
            }
            foreach (var book in removedBooks)
            {                
                _bookContext.BookChanges.Add(new BookChanges
                {
                    Title = book.Title,
                    Author = book.Author,
                    ChangeDate = DateTime.Now,
                    ChangeType = "Removed"
                });
            }
            await _bookContext.SaveChangesAsync(); 

        }
    }
    public class BooksComparer : IEqualityComparer<BookModel>
    {
        public bool Equals(BookModel? x, BookModel? y)
        {
            return x.Title==y.Title && x.Author==y.Author;
        }

        public int GetHashCode([DisallowNull] BookModel obj)
        {
            return HashCode.Combine(obj.Title, obj.Author);
        }
    }
}
