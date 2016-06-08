using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ShopSStorage.Schemats;

namespace ShopSStorage.Models
{
    public sealed class BusinessDbContext : IDisposable
    {

        private bool _disposed;
        private readonly ShopSStorageDbContext _context;

        public BusinessDbContext()
        {
            _context = new ShopSStorageDbContext();
        }

        public ShopSStorageDbContext ShopSStorageDbContext
        {
            get { return _context; }
        }

        public ICollection<Cathegory> GetCathegories()
        {
            return _context.Cathegories.OrderBy(n => n.CathegoryName).ToList<Cathegory>();
        }

        public void DeleteCathegory(Cathegory cathegory)
        {
            _context.Cathegories.Remove(cathegory);
            _context.SaveChanges();
        }

        public void AddNewCathegory(Cathegory cathegory)
        {
            _context.Cathegories.Add(cathegory);
            _context.SaveChanges();
        }

        public void AddNewProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public ICollection<Product> GetProducts(Cathegory cathegory)
        {
            return _context.Cathegories.Where(c =>c.CathegoryId == cathegory.CathegoryId).Select(p => p.Products).Single().ToList<Product>();
        }
        #region Idisposable member

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposingStateent)
        {
            if (_disposed || !disposingStateent)
                return;
            if (_context != null)
                _context.Dispose();
            _disposed = true;
        }

        #endregion
    }
}