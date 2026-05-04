# Mercato260501

Web shop application to sell t-shirts.

## MVP scope

This document defines the minimum viable product scope for the Mercato t-shirt store.

### 1. User management

In scope:
- Customer registration
- Login and logout
- Password reset
- User profile with name, email, phone number, and delivery addresses
- Basic roles: customer and admin
- Order history for signed-in users

Out of scope:
- Social login
- Multi-factor authentication
- Loyalty program

### 2. Product catalog

In scope:
- Product listing page
- Product detail page
- Category browsing and filtering
- Search and sort
- T-shirt variants for size and color
- Product price, stock status, images, and descriptions

Out of scope:
- Recommendations
- Reviews
- Bundles

### 3. Cart module

In scope:
- Add items to cart
- Remove items from cart
- Update quantity
- Select and update product variants
- Persist cart for guest and signed-in users
- Cart totals
- Taxes and discounts placeholder

Out of scope:
- Saved carts
- Advanced promotions engine

### 4. Delivery

In scope:
- Delivery address capture
- Shipping method selection
- Shipping cost calculation
- Order status flow: pending, packed, shipped, delivered

Out of scope:
- Live courier integrations
- Split shipments

### 5. Reports

In scope:
- Sales summary by day, week, and month
- Top-selling products
- Order count
- Revenue
- Average basket value
- Inventory snapshot

Out of scope:
- Custom report builder
- BI dashboards

### 6. Admin panel

In scope:
- Manage products, variants, prices, and stock
- Manage orders and update fulfillment status
- Manage users with basic admin controls
- View core reports

Out of scope:
- Fine-grained permissions
- Audit trail
- CMS features

## Cross-cutting scope

- Responsive storefront
- Basic checkout flow
- Validation and error handling
- Email notifications
- Authentication and authorization
- Basic analytics and logging

## Assumptions

- The first release is an MVP focused on selling t-shirts only.
- Payments are not yet defined and need confirmation before implementation.
- Delivery can start with a simple internal shipping model unless courier integration is requested.

## Open questions

- Is this strictly MVP, or should phase 2 features be included now?
- Should online payments be included in scope?
- Should guests be allowed to checkout without registration?
- Is multilingual or multi-currency support required?
- Should delivery use a fixed-rate model or an external courier integration?
