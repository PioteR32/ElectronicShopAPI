using Shared.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopAPI.Services.Interfaces
{
	public interface ICommentService
	{
		Task<IEnumerable<CommentDTO>> GetCommentsForProduct(int productId);
		Task<CommentDTO?> GetById(int id);
		Task<int> Create(CommentDTO dto);
		Task<bool> Update(int id, CommentDTO dto);
		Task<bool> Delete(int id);
	}
}
