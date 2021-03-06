﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows.Input;

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

        #region Operation on Cathegories
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
            _context.Cathegories.AddOrUpdate(cathegory);
            _context.SaveChanges();
        }
        #endregion

        #region Operation on Products
        public void ChangeProductStorageAmount(Product product, int soldAmount)
        {
            if (soldAmount > 0)
            {
                product.StorageAmount -= soldAmount;
                _context.Entry(product).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
        }
        public void AddNewProduct(Product product)
        {
            _context.Products.AddOrUpdate(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
        public ICollection<Product> GetProducts(Cathegory cathegory)
        {
            return _context.Cathegories.Where(c =>c.CathegoryId == cathegory.CathegoryId).Select(p => p.Products).Single().ToList<Product>();
        }
        #endregion

        #region Operation on SalesHistories
        public void AddSalesHistories(ICollection<SalesHistory> salesHistories)
        {
            foreach (var obj in salesHistories)
            {
                _context.SalesHistories.Add(obj);
            }
            _context.SaveChanges();
        }
        #endregion

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