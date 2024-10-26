using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entities.Product;
using Store.Core.Repositories.Contract;

namespace Store.DashBoard.Controllers
{
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> Index()
        {
            var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync();

            return View(brands);
        }

        public async Task<IActionResult> Create(ProductBrand brand)
        {

            var allBrands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync();

            if (!allBrands.Any(b => b.Name.ToLower() == brand.Name.ToLower()))
            {
                await _unitOfWork.Repository<ProductBrand, int>().AddAsync(brand);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                ModelState.AddModelError("Name", "Please Enter New Brand");
                return View("Index", await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync());
            }

            return RedirectToAction("Index");




        }

        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _unitOfWork.Repository<ProductBrand, int>().GetByIdAsync(id);

            _unitOfWork.Repository<ProductBrand, int>().Delete(brand);

            await _unitOfWork.CompleteAsync();

            return RedirectToAction("Index");
        }
    }
}
