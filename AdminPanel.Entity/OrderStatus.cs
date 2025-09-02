namespace AdminPanel.Entity;

public enum OrderStatus
{
    Open,      // Games are in the cart
    Checkout,  // Payment is started
    Paid,      // Payment is performed successfully
    Cancelled, // Payment is performed with errors
    Shipped,   // Order has been shipped
}
