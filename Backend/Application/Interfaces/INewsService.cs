using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public interface INewsService
    {
        public Task AddAsync(Domain.Entities.News.News news);
        public Task<bool> DeleteAsync(int? id);
        public Task<bool> EditAsync(Domain.Entities.News.News news);
        public IEnumerable<Domain.Entities.News.News> GetNews(int page = 0, int count = 0);

    }
}
