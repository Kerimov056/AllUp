using AllupProject.Business.Interfaces;
using AllupProject.CustomExceptions.Common;
using AllupProject.DAL;
using AllupProject.Models;
using AllupProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace AllupProject.Controllers;

public class HomeController : Controller
{
    private readonly AllupDbContext _context;
    private readonly ICartService _cartService;

    public HomeController(AllupDbContext context, ICartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }
    public async Task<IActionResult> Index()
    {
        HomeViewModel homeViewModel = new HomeViewModel()
        {
            Sliders = await _context.Sliders.Where(x => x.IsDeactive == false).ToListAsync(),
            Banners = await _context.Banners.Where(x => x.IsDeactive == false).ToListAsync(),
            Categories = await _context.Categories.Where(x => x.IsDeactive == false).ToListAsync(),
            Blogs = await _context.Blogs.Where(x => x.IsDeactive == false).ToListAsync(),
            Products = await _context.Products.Where(x => x.IsDeactive == false).Include(x => x.ProductImages).Include(x => x.Category).ToListAsync()
        };
        return View(homeViewModel);
    }
    public IActionResult About() => View();
    public IActionResult Blog() => View();
    public IActionResult Shop() => View();
    public async Task<IActionResult> Detail(int productId) {
        Product product=await _context.Products.Include(x=>x.ProductImages).Include(x=>x.Category).FirstOrDefaultAsync(x=>x.Id==productId);
        if (product is null) return NotFound();
        return View(product);
    }



    public async Task<IActionResult> Cart()
    {
        try
        {
            List<CartItemViewModel> cartItems = await _cartService.Cart(HttpContext);
            ViewBag.Products = _context.Products.Include(x => x.ProductImages).ToList();
            return View(cartItems);
        }
        catch (Exception)
        {
            return NotFound();
        }

    }

}

