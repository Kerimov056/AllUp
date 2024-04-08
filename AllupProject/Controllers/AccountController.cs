﻿using AllupProject.Business.Interfaces;
using AllupProject.CustomExceptions.Common;
using AllupProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AllupProject.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accService;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(IAccountService accService,UserManager<IdentityUser> userManager)
    {
        _accService = accService;
        _userManager = userManager;
    }
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(UserRegisterViewModel userRegisterViewModel)
    {
        if (!ModelState.IsValid)return View();
        try
        {
            await _accService.RegisterAsync(userRegisterViewModel);
        }
        catch (NameAlreadyExistException ex)
        {
            ModelState.AddModelError(ex.PropertyName,ex.Message);
            return View();
        }
        catch (NotSucceededException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("",ex.Message);
            return View();
        }
            return RedirectToAction("index","home");
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
    {
        if (!ModelState.IsValid) return View();
        try
        {
            await _accService.LoginAsync(userLoginViewModel);
        }
        catch (EntityCannotBeFoundException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        } 
        catch (NotSucceededException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
       await _accService.LogoutAsync();

        return RedirectToAction("Login", "Account");
    }

}
