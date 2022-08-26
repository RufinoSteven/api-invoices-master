using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiInvoices.Context;
using ApiInvoices.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiInvoices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoicesContext _context;
        public InvoicesController (InvoicesContext _context)
        {
            this._context= _context;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(_context.Invoices.ToList());


            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}", Name = "getInvoices")]
        public ActionResult Get(int id)
        {
            try
            {
                var gInvoices = _context.Invoices.FirstOrDefault(x => x.id == id);
                return Ok(gInvoices);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public ActionResult Post([FromBody] Invoices gInvoices)
        {
            Invoices inv = null;
            try
            {
                _context.Invoices.Add(gInvoices);
                _context.SaveChanges();
                return Ok();
              
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            
        }
        [HttpPost]
        [Route("api/Detalle")]
        public ActionResult Post([FromBody] Detalle detalle)
        {
            try
            {
                _context.Detalles.Add(detalle);
                _context.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDetalle/{idFactura}")]
        public ActionResult GetDetalle(int idFactura)
        {
            try
            {
                var itemList = _context.Detalles.Where(x => x.analisis_id == idFactura).Select(c => new
                {
                    item = c.item_id,
                    qty = c.qty
                }).ToList();
                return Ok(itemList);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Invoices gInvoices)
        {
            try
            {
                if (gInvoices.id == id)
                {
                    _context.Entry(gInvoices).State= EntityState.Modified;
                    _context.SaveChanges();
                    return Ok();

                }
                else
                {
                    return BadRequest();
                }
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var gInvoices = _context.Invoices.FirstOrDefault(X=> X.id==id);
                if (gInvoices != null)
                {
                    var itemList = _context.Detalles.Where(x => x.analisis_id == id).Select(c => new
                    {
                        detalle = c.id,
                        item = c.item_id,
                        qty = c.qty
                    }).ToList();
                    foreach (var item in itemList)
                    {
                        var gItem = await _context.Detalles.FirstOrDefaultAsync(X => X.id == item.detalle);
                        _context.Detalles.Remove(gItem);
                        await _context.SaveChangesAsync();
                    }
                    _context.Invoices.Remove(gInvoices);
                    await _context.SaveChangesAsync();
                    return Ok(id);

                }
                else
                {
                    return BadRequest();
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
