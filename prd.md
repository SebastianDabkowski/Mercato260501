# Product Requirements Document (PRD)

## 1. Product overview

Mercato is a web shop focused on selling t-shirts online. The MVP should deliver a responsive storefront, a basic purchasing flow, and a simple admin experience for operating the catalog, orders, users, and core reports.

## 2. Product goal

Launch a minimum viable e-commerce product that allows customers to discover t-shirts, manage a cart, place orders, and track their order history, while enabling admins to manage products, stock, orders, users, and reporting.

## 3. MVP scope

### 3.1 User management

#### In scope
- Customer registration
- Login and logout
- Password reset
- User profile with:
  - Name
  - Email
  - Phone number
  - Delivery addresses
- Basic roles:
  - Customer
  - Admin
- Order history for signed-in users

#### Out of scope
- Social login
- Multi-factor authentication
- Loyalty program

### 3.2 Product catalog

#### In scope
- Product listing page
- Product detail page
- Category browsing and filtering
- Search
- Sort
- T-shirt variants:
  - Size
  - Color
  - Price
  - Stock status
- Product images
- Product descriptions

#### Out of scope
- Recommendations
- Reviews
- Bundles

### 3.3 Cart module

#### In scope
- Add items to cart
- Remove items from cart
- Update quantity
- Update variant selection
- Persist cart for:
  - Guest users
  - Signed-in users
- Cart totals
- Taxes placeholder
- Discounts placeholder if needed

#### Out of scope
- Saved carts
- Advanced promotions engine

### 3.4 Delivery

#### In scope
- Delivery address capture
- Shipping method selection
- Shipping cost calculation
- Order status flow:
  - Pending
  - Packed
  - Shipped
  - Delivered

#### Out of scope
- Live courier integrations
- Split shipments

### 3.5 Reports

#### In scope
- Sales summary by:
  - Day
  - Week
  - Month
- Top-selling products
- Order count
- Revenue
- Average basket value
- Inventory snapshot

#### Out of scope
- Custom report builder
- BI dashboards

### 3.6 Admin panel

#### In scope
- Manage products
- Manage variants
- Manage pricing
- Manage stock
- Manage orders
- Update fulfillment status
- Manage users with basic admin controls
- View core reports

#### Out of scope
- Fine-grained permissions
- Audit trail
- CMS features

## 4. Cross-cutting requirements

- Responsive storefront
- Basic checkout flow
- Validation and error handling
- Email notifications
- Security baseline:
  - Authentication
  - Authorization
  - Input validation
- Basic analytics and logging

## 5. Assumptions

- This release is strictly an MVP for a t-shirt-only storefront.
- The MVP should prioritize essential buying and operational flows over advanced commerce features.
- Payments are not confirmed and require a product decision before implementation.
- Delivery can begin with a simple internal shipping model unless external courier integration is explicitly required.

## 6. Open questions

- Is this strictly MVP, or should phase 2 features be included now?
- Should online payments be included in scope?
- Should guests be allowed to checkout without registration?
- Is multilingual support required?
- Is multi-currency support required?
- Should delivery use a fixed-rate model or an external courier integration?

## 7. Success criteria for MVP

- Customers can create accounts, sign in, manage their profile, and view order history.
- Customers can browse, search, filter, and view t-shirt products and variants.
- Customers can add products to a cart, update quantities or variants, and continue checkout with cart persistence.
- Delivery details and shipping method can be captured during checkout.
- Orders can move through the defined fulfillment statuses.
- Admins can manage catalog data, stock, orders, users, and view the core reports.
