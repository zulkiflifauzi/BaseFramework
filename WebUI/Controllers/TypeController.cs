﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Domain.Interfaces;
using AutoMapper;
using Repository;
using WebUI.Models;
using Base;

namespace WebUI.Controllers
{
    public class TypeController : BaseController
    {
        private readonly ITypeService _typeService;

        [Inject]
        public TypeController(ITypeService typeService)
        {
            _typeService = typeService;
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {            
            var models = Mapper.Map<List<Type>, List<ViewModelType>>(_typeService.GetAll());
            return View(models);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Create(ViewModelType model)
        {
            var entity = Mapper.Map<ViewModelType, Type>(model);
            _typeService.Add(entity);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            return View(Mapper.Map<Type, ViewModelType>(_typeService.GetTypeById(id)));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(ViewModelType model)
        {
            var entity = Mapper.Map<ViewModelType, Type>(model);
            ResponseMessage response =  _typeService.Update(entity);

            if (response.IsError == true)
            {
                foreach (var item in response.ErrorCodes)
                {
                    ModelState.AddModelError(item, item);
                }

                return View();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            ResponseMessage response = _typeService.Delete(id);

            if (response.IsError == true)
            {
                foreach (var item in response.ErrorCodes)
                {
                    ModelState.AddModelError(item, item);
                }

                return View();
            }
            return RedirectToAction("Index");
        }

    }
}
