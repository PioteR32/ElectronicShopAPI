using ElectronicShopAPI.Data;
using ElectronicShopAPI.Models;
using Shared.DTO;
using Microsoft.EntityFrameworkCore;
using ElectronicShopAPI.Services.Interfaces;

namespace ElectronicShopAPI.Services.Classes
{
	public class CommentService : ICommentService
	{
		private readonly DataContext _context;
		public CommentService(DataContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<CommentDTO>> GetCommentsForProduct(int productId)
		{
			return await _context.Comments
				.Where(c => c.ProductId == productId)
				.Include(c => c.Customer)
				.Select(c => new CommentDTO { Id = c.Id, Comment = c.Comment1, Rating = c.Rating, ProductId = c.ProductId, CustomerId = c.CustomerId, CustomerFirstName = c.Customer.FirstName, CustomerLastName = c.Customer.LastName })
				.ToListAsync();
		}

		public async Task<CommentDTO?> GetById(int id)
		{
			var c = await _context.Comments.Include(c => c.Customer).FirstOrDefaultAsync(c => c.Id == id);
			if (c == null) return null;
			return new CommentDTO { Id = c.Id, Comment = c.Comment1, Rating = c.Rating, ProductId = c.ProductId, CustomerId = c.CustomerId, CustomerFirstName = c.Customer.FirstName, CustomerLastName = c.Customer.LastName };
		}

		public async Task<int> Create(CommentDTO dto)
		{
			var c = new Comment { Comment1 = dto.Comment, Rating = dto.Rating, ProductId = dto.ProductId, CustomerId = dto.CustomerId };
			_context.Comments.Add(c);
			await _context.SaveChangesAsync();
			return c.Id;
		}

		public async Task<bool> Update(int id, CommentDTO dto)
		{
			var c = await _context.Comments.FindAsync(id);
			if (c == null) return false;
			c.Comment1 = dto.Comment;
			c.Rating = dto.Rating;
			c.ProductId = dto.ProductId;
			c.CustomerId = dto.CustomerId;
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> Delete(int id)
		{
			var c = await _context.Comments.FindAsync(id);
			if (c == null) return false;
			_context.Comments.Remove(c);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
