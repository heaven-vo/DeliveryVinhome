using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

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
                if (result != null)
                {
                    await repository.Segment.CreatSegment(result);
                }
                return Ok(new { StatusCode = "Successful", data = result });
            }
            catch
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = "Hiện tại cửa hàng đã ngưng hoạt động !!" +
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
                await repository.Order.OrderUpdateStatus(orderId, order.StatusId);
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
        [HttpGet("ByOrderId/payment")]
        public async Task<ActionResult> GetPaymentOrder(string orderId)
        {
            try
            {
                var pro = await repository.Order.PaymentOrder(orderId);

                return Redirect(pro.ToString());
            }
            catch
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = "Loại hình thanh toán của đơn hàng không hợp lệ !! Vui lòng kiểm tra và thử lại"
                });
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
            VnPayLibrary pay = new VnPayLibrary();
            var vnpayData = Request.QueryString.ToString();
            Console.WriteLine(vnpayData);
            //lấy toàn bộ dữ liệu được trả về
            string[] authorsList = vnpayData.Split("&");
            string vnp_Amount = "";
            string vnp_BankCode = "";
            string vnp_BankTranNo = "";
            string vnp_CardType = "";
            string vnp_OrderInfo = "";
            string vnp_PayDate = "";
            string vnp_ResponseCode = "";
            string vnp_TmnCode = "";
            string vnp_TransactionNo = "";
            string vnp_TransactionStatus = "";
            string vnp_TxnRef = "";

            Console.WriteLine("authorsList" + String.Join(Environment.NewLine, authorsList));
            foreach (string author in authorsList)
            {

                string[] ListRespone = author.Split("=");
                Console.WriteLine("ListRespone[0]" + ListRespone[0]);
                Console.WriteLine("ListRespone[1]" + ListRespone[1]);
                if (ListRespone[0] == "?vnp_Amount")
                {
                    vnp_Amount = ListRespone[1];
                }
                else if (ListRespone[0] == "vnp_BankCode")
                {
                    vnp_BankCode = ListRespone[1];
                }
                else if (ListRespone[0] == "vnp_BankTranNo")
                {
                    vnp_BankTranNo = ListRespone[1];
                }
                else if (ListRespone[0] == "vnp_CardType")
                {
                    vnp_CardType = ListRespone[1];
                }
                else if (ListRespone[0] == "vnp_OrderInfo")
                {
                    vnp_OrderInfo = ListRespone[1];
                }
                else if (ListRespone[0] == "vnp_PayDate")
                {
                    vnp_PayDate = ListRespone[1];
                }
                else if (ListRespone[0] == "vnp_ResponseCode")
                {
                    vnp_ResponseCode = ListRespone[1];
                }
                else if (ListRespone[0] == "vnp_TmnCode")
                {
                    vnp_TmnCode = ListRespone[1];
                }
                else if (ListRespone[0] == "vnp_TransactionNo")
                {
                    vnp_TransactionNo = ListRespone[1];
                }
                else if (ListRespone[0] == "vnp_TransactionStatus")
                {
                    vnp_TransactionStatus = ListRespone[1];
                }
                else if (ListRespone[0] == "vnp_TxnRef")
                {
                    vnp_TxnRef = ListRespone[1];
                }

            }


            string vnp_SecureHash = Request.Query["vnp_SecureHash"]; //hash của dữ liệu trả về

            string hashRaw = "vnp_Amount=" + vnp_Amount + "&" + "vnp_BankCode=" + vnp_BankCode + "&" + "vnp_BankTranNo=" + vnp_BankTranNo + "&" + "vnp_CardType=" +
                vnp_CardType + "&" + "vnp_OrderInfo=" + vnp_OrderInfo + "&" + "vnp_PayDate=" + vnp_PayDate + "&" + "vnp_ResponseCode=" + vnp_ResponseCode + "&" + "vnp_TmnCode=" + vnp_TmnCode + "&" + "vnp_TransactionNo=" + vnp_TransactionNo +
                "&" + "vnp_TransactionStatus=" + vnp_TransactionStatus + "&" + "vnp_TxnRef=" + vnp_TxnRef;

            Console.WriteLine("vnp_ResponseCode" + vnp_ResponseCode);
            bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret, hashRaw); //check chữ ký đúng hay không?

            string hashRaw2 = "vnp_Amount=" + vnp_Amount + "&" + "vnp_BankCode=" + vnp_BankCode + "&" + "vnp_BankTranNo=" + vnp_BankTranNo + "&" + "vnp_CardType=" +
                vnp_CardType + "&" + "vnp_OrderInfo=" + vnp_OrderInfo + "&" + "vnp_PayDate=" + vnp_PayDate + "&" + "vnp_ResponseCode=" + vnp_ResponseCode + "&" + "vnp_TmnCode=" + vnp_TmnCode + "&" + "vnp_TransactionNo=" + vnp_TransactionNo +
                "&" + "vnp_TransactionStatus=" + vnp_TransactionStatus + "&" + "vnp_TxnRef=" + vnp_TxnRef;

            Console.WriteLine("checkSignature" + checkSignature);
            Console.WriteLine("vnp_TxnRef" + vnp_TxnRef);
            if (checkSignature)
            {
                if (vnp_ResponseCode == "00")
                {
                    //Thanh toán thành công
                    var order = await repository.Order.PaymentOrderSuccessfull(vnp_TxnRef);
                    string Successfull = "https://foodd-delivery.netlify.app/order/" + vnp_TxnRef;
                    return Redirect(Successfull);
                }
                else
                {
                    var faild = await repository.Order.PaymentOrderFalse(vnp_TxnRef);
                    string Failed = "https://foodd-delivery.netlify.app/order/payment/failed";
                    return Redirect(Failed);
                }

            }
            else
            {
                var faild = await repository.Order.PaymentOrderFalse(vnp_TxnRef);
                string Failed = "https://foodd-delivery.netlify.app/order/payment/failed";
                return Redirect(Failed);
            }
            return Ok();
        }
        [HttpGet("complete")]
        public async Task<ActionResult> CompleteOrder(string orderActionId, string shipperId, int actionType)
        {
            try
            {
                await repository.Order.CompleteOrder(orderActionId, shipperId, actionType);
                return Ok(new { StatusCode = "Successful" });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = e.Message
                });
            }
        }
        [HttpGet("cancel")]
        public async Task<ActionResult> CancelOrder(string orderActionId, string shipperId, int actionType)
        {

            return Ok();
        }

    }
}
