using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Infrastructure.Services;
using DeliveryVHGP.Core.Data;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public OrdersController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// Get list orders (customer web)
        /// </summary>
        // GET: api/Orders
        [HttpGet("{cusId}/customers")]
        public async Task<ActionResult> GetOrder(string cusId, int pageIndex, int pageSize)
        {
            var listOder = await repository.Order.GetListOrders(cusId, pageIndex, pageSize);
            if (cusId == null)
                return NotFound();
            return Ok(listOder);
        }
        /// <summary>
        /// Get list orders by store (customer web)
        /// </summary>
        // GET: api/Orders
        [HttpGet("stores/byStoreId")]
        public async Task<ActionResult> GetOrderByStore(string storeId, int pageIndex, int pageSize)
        {
            var listOder = await repository.Order.GetListOrdersByStore(storeId, pageIndex, pageSize);
            return Ok(listOder);
        }
        /// <summary>
        /// Get list orders by store (store web)
        /// </summary>
        // GET: api/Orders
        [HttpGet("stores/byStoreId/status/ByStatusId")]
        public async Task<ActionResult> GetOrderByStoreByStatus(int statusId, string storeId, int pageIndex, int pageSize)
        {
            var listOder = await repository.Order.GetListOrdersByStoreByStatus(storeId, statusId, pageIndex, pageSize);
            if (storeId == null)
                return NotFound();
            return Ok(listOder);
        }
        /// <summary>
        /// Get order by id with pagination
        /// </summary>
        //GET: api/v1/orderById?pageIndex=1&pageSize=3
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderDetail(string id)
        {
            try
            {
                var pro = await repository.Order.GetOrdersById(id);
                return Ok(pro);
            }
            catch
            {
                return NotFound();
            }
        }
        /// <summary>
        /// Create a order (customer web)
        /// </summary>
        //POST: api/v1/order
        [HttpPost]
        public async Task<ActionResult> CreatNewOrder(OrderDto order)
        {
            try
            {
                var result = await repository.Order.CreatNewOrder(order);
                return Ok(new { StatusCode = "Successful", data = result });
            }
            catch
            {
                return Ok(new
                {
                    StatusCode = "Fail", message = "Hiện tại cửa hàng đã ngưng hoạt động !!" +
                                              "Vui lòng đặt lại đơn hàng "
                });
            }
        }
        /// <remarks>
        /// Status in body
        /// CreateOrder = 1, ShipAccept = 2, Shipping = 3, Done = 4, Cancel = 5, ShipCancel = 6, ShipCancelOther = 7, ShopCancel = 8, CustomerCancel = 9, CustomerPayCancel = 10
        /// </remarks>
        /// <summary>
        /// Update a status order (customer web)
        /// </summary>
        //POST: api/v1/order
        [HttpPut("{orderId}")]
        public async Task<ActionResult<OrderStatusModel>> UpdateOrder(string orderId, OrderStatusModel order)
        {
            if (orderId != order.OrderId)
            {
                return BadRequest("Order Id mismatch");
            }
            if (order.OrderId == null)
            {
                return NotFound("Order Id does not exist");
            }
            try
            {
                await repository.Order.OrderUpdateStatus(orderId, order);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
            return Ok(order);
        }
        /// <summary>
        /// Get TimeDuration By MenuId in Mode3 (customer web)
        /// </summary>
        //GET: api/v1/order?pageIndex=1&pageSize=3 
        [HttpGet("ByMenuId")]
        public async Task<ActionResult> GetTimeDuration(string menuId, int pageIndex, int pageSize)
        {
            return Ok(await repository.Order.GetDurationOrder(menuId, pageIndex, pageSize));
        }
        /// <summary>
        /// Get order by id with pagination
        /// </summary>
        //GET: api/v1/orderById?pageIndex=1&pageSize=3
        [HttpGet("ByOrderId")]
        public async Task<ActionResult> GetPaymentOrder(string orderId)
        {
            try
            {
                var pro = await repository.Order.PaymentOrder(orderId);

                return Ok(pro);
            }
            catch
            {
                return NotFound();
            }
     
        }
        /// <summary>
        /// Get order by id with pagination
        /// </summary>
        //GET: api/v1/orderById?pageIndex=1&pageSize=3
        [HttpGet("Payment-confirm")]
        public async Task<ActionResult> GetPaymentConfirm()
        {
                string hashSecret = "YLGGIJRNXHISHHCZSMHXFRVXUTJIFMSZ"; //Chuỗi bí mật
                                                                        //string vnpayData = HttpContext.Request.QueryString.ToString();
            VnPayLibrary pay = new VnPayLibrary();
            var vnpayData = Request.QueryString.ToString();
            Console.WriteLine(vnpayData);
            //lấy toàn bộ dữ liệu được trả về

            //foreach (string s in vnpayData)
            //{
            //    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
            //    {
            //        pay.AddResponseData(s, vnpayData[s]);
            //    }
            //}
            string[] authorsList = vnpayData.Split("&");
            string vnp_Amount = "";
            string vnp_Command = "pay";
            string vnp_CreateDate = "20221115143237";
            string vnp_CurrCode = "VND";
            string vnp_Locale = "vn";
            string vnp_OrderInfo = "";
            string vnp_OrderType = "other";
            string vnp_ReturnUrl = "https://localhost:7102/api/v1/orders/Payment-confirm";
            string vnp_TmnCode = "";
            string vnp_TxnRef = "";
            string vnp_Version = "2.1.0";
            Console.WriteLine("authorsList"+ String.Join(Environment.NewLine, authorsList));
            foreach(string author in authorsList)
            {
                
                string[] ListRespone = author.Split("=");
                Console.WriteLine("ListRespone[0]" + ListRespone[0]);
                Console.WriteLine("ListRespone[1]" + ListRespone[1]);
                if (ListRespone[0] == "?vnp_Amount")
                {
                    vnp_Amount = ListRespone[1];
                }
                //else if(ListRespone[0] == "vnp_CreateDate")
                //{
                //    vnp_CreateDate = ListRespone[1];
                //}
                //else if(ListRespone[0] == "vnp_CurrCode")
                //{
                //    vnp_CurrCode = ListRespone[1];
                //}
                //else if(ListRespone[0] == "vnp_Locale")
                //{
                //    vnp_Locale = ListRespone[1];
                //} 
                else if(ListRespone[0] == "vnp_OrderInfo")
                {
                    vnp_OrderInfo = ListRespone[1];
                }
                else if(ListRespone[0] == "vnp_TmnCode")
                {
                    vnp_TmnCode = ListRespone[1];
                }
                //else if (ListRespone[0] == "vnp_OrderType")
                //{
                //    vnp_OrderType = ListRespone[1];
                //} 
                //else if (ListRespone[0] == "vnp_ReturnUrl")
                //{
                //    vnp_ReturnUrl = ListRespone[1];
                //}
                else if (ListRespone[0] == "vnp_TxnRef")
                {
                    vnp_TxnRef = ListRespone[1];
                } 
                else if (ListRespone[0] == "vnp_Version")
                {
                    vnp_Version = ListRespone[1];
                }
            }
            //string orderId = pay.GetResponseData("vnp_TxnRef"); //mã hóa đơn
            //string vnpayTranId = pay.GetResponseData("vnp_TransactionNo"); //mã giao dịch tại hệ thống VNPAY
            //pay.GetResponseData
            string vnp_ResponseCode = Request.Query["vnp_ResponseCode"];//response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
            string vnp_SecureHash = Request.Query["vnp_SecureHash"]; //hash của dữ liệu trả về


            string hashRaw = vnp_Amount + "&" + vnp_Command + "&" + vnp_CreateDate + "&" + vnp_CurrCode + "&" + vnp_Locale + "&" + vnp_OrderInfo + "&" + vnp_OrderType + "&"     + vnp_ReturnUrl + "&" + vnp_TmnCode + "&" + vnp_TxnRef +"&" + vnp_Version;
            Console.WriteLine("hashRaw"+ hashRaw);
            Console.WriteLine("vnp_Amount"+vnp_Amount);
            bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?
            Console.WriteLine(checkSignature);
            if (checkSignature )
            {
                if (vnp_ResponseCode == "00")
                {
                    //Thanh toán thành công
                    throw new Exception ("Successful" + vnp_SecureHash );

                }
                else
                {
                    //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                    throw new Exception("Faild" + vnp_SecureHash);
                }
                
            }
            return Ok(vnpayData);
        }


    }
}
