using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Application;
using Microsoft.EntityFrameworkCore;
using Domain.Context;
using Domain.Contexts;
using DAL = Domain.Entities.News;

namespace Infrastructure.Services
{
    public class NewsService : INewsService
    {
        DataContext _context;
        public NewsService(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DAL.News news)
        {
            await _context.AddAsync(news);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int? id)
        {
            var existingNews = await _context.News.FindAsync(id);

            if(existingNews != null)
            {
                _context.Remove(existingNews);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;   
        }

        public async Task<bool> EditAsync(DAL.News news)
        {
            var existingNews = await _context.News.FindAsync(news.Id);

            if(existingNews != null)
            {
                _context.Entry(existingNews).CurrentValues.SetValues(news);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public IEnumerable<DAL.News> GetNews(int page = 0, int count = 0)
        {
            IEnumerable<DAL.News> news = _context.News.AsNoTracking().Skip(page * count).Take(count);

            return news;
        }
    }
}
