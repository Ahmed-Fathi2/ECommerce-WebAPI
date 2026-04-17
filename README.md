# ECommerce
# E-Commerce API — Developer README

> **ASP.NET Core | EF Core | JWT | Clean Architecture | N-Tier**

---

## ⚠️ Read This First — Golden Rule

> **If you find something already implemented — USE IT. Do NOT rewrite it.**

This project has established patterns for everything. Before you write a single line, scan the existing code first. If a pattern exists, follow it exactly. Consistency is more important than your personal preference.

---

## Implementation Phases

| # | Phase | Key Output |
|---|-------|-----------|
| 1 | Project Foundation & Architecture | Solution structure, Entities, DbContext, Repositories, UoW, Common layer |
| 2 | Authentication & Authorization | Register, Login, JWT, Policies, Claims extraction |
| 3 | Category & Product Management | CRUD, filtering, pagination, image upload |
| 4 | Cart Management | Add / remove / update / get — fully user-scoped |
| 5 | Order Management | Place order, history, details — atomic UoW transaction |
Swagger |

Complete each phase fully and pass its checklist before starting the next.

---

## The Core Rule: Find It First, Use It As-Is

Before implementing **anything**, ask yourself:

> *"Does this already exist somewhere in the codebase?"*

If **yes** → use it exactly as it is. Do not rename it, do not refactor it, do not "improve" it.  
If **no** → implement it following the exact same pattern as the closest existing thing.

The sections below tell you what already exists and how to use each one.

---

## 1. Result Pattern — `ApiResponse<T>`

**Location:** `ECommerce.Common/Wrappers/ApiResponse.cs`

If you find this class, **every single service method and controller action must use it**. No exceptions.

### How to use it (service layer):

```csharp
// ✅ Correct — always return ApiResponse<T>
public async Task<ApiResponse<ProductDto>> GetProductAsync(int id)
{
    var product = await _unitOfWork.Products.GetByIdAsync(id);
    if (product == null)
        return ApiResponse<ProductDto>.Failure("Product not found.");

    return ApiResponse<ProductDto>.Success(_mapper.Map<ProductDto>(product));
}
```

```csharp
// ❌ Wrong — never throw business exceptions, never return raw objects
public async Task<ProductDto> GetProductAsync(int id)
{
    var product = await _unitOfWork.Products.GetByIdAsync(id);
    if (product == null)
        throw new NotFoundException("Product not found"); // ← NEVER do this
    return _mapper.Map<ProductDto>(product);
}
```

### How to use it (controller layer):

```csharp
// ✅ Correct — map the result to the right HTTP status code
[HttpGet("{id}")]
public async Task<IActionResult> GetProduct(int id)
{
    var result = await _productService.GetProductAsync(id);
    return result.Success ? Ok(result) : NotFound(result);
}
```

**Rules:**
- `200 OK` → `Ok(result)` when `result.Success == true`
- `400 Bad Request` → `BadRequest(result)` for validation / business errors
- `401 Unauthorized` → handled by middleware (you don't return this manually)
- `403 Forbidden` → handled by `[Authorize]` attribute
- `404 Not Found` → `NotFound(result)` when entity doesn't exist
- `500` → **only** from the global exception handler, never from your code

---

## 2. Unit of Work — `IUnitOfWork`

**Location:** `ECommerce.DAL/UnitOfWork/IUnitOfWork.cs`

If it exists, **inject `IUnitOfWork` into every service**. Never inject individual repositories directly into services.

### How to use it:

```csharp
// ✅ Correct — single UoW commit covers all changes atomically
public async Task<ApiResponse<string>> PlaceOrderAsync(string userId, PlaceOrderDto dto)
{
    var cartItems = await _unitOfWork.CartItems.GetUserCartAsync(userId);
    if (!cartItems.Any())
        return ApiResponse<string>.Failure("Cart is empty.");

    var order = new Order { UserId = userId, TotalAmount = cartItems.Sum(x => x.Total) };
    await _unitOfWork.Orders.AddAsync(order);

    foreach (var item in cartItems)
    {
        await _unitOfWork.OrderItems.AddAsync(new OrderItem { ... });
        item.Product.StockQuantity -= item.Quantity;  // decrement stock
    }

    _unitOfWork.CartItems.RemoveRange(cartItems);  // clear cart

    await _unitOfWork.CommitAsync();  // ← single transaction, all or nothing
    return ApiResponse<string>.Success("Order placed successfully.");
}
```

```csharp
// ❌ Wrong — never call SaveChangesAsync() on the DbContext directly from a service
await _context.SaveChangesAsync();
```

**Rule:** One `CommitAsync()` call per operation. Never call it multiple times inside the same method.

---

## 3. Generic Repository — `IGenericRepository<T>`

**Location:** `ECommerce.DAL/Repositories/Generic/IGenericRepository.cs`

If it exists, the specific repositories **extend** it. You get `GetAll`, `GetByIdAsync`, `AddAsync`, `Update`, `Delete`, `SaveAsync` for free.

### How to use specific repos:

```csharp
// ✅ Correct — use the repo through UoW
var product = await _unitOfWork.Products.GetByIdAsync(id);
await _unitOfWork.Products.AddAsync(newProduct);
_unitOfWork.Products.Update(existingProduct);
_unitOfWork.Products.Delete(product);
await _unitOfWork.CommitAsync();
```

```csharp
// ❌ Wrong — never inject DbContext into a service
public class ProductService
{
    private readonly AppDbContext _context; // ← NEVER
}
```

**If a method you need doesn't exist on the generic repo**, add it to the specific repository interface (e.g., `IProductRepository`) and implement it there. Do not modify `IGenericRepository<T>`.

---

## 4. UserId Extraction — From JWT Claims Only

**Location:** `ECommerce.API/Extensions/ClaimsPrincipalExtensions.cs` (or similar)

If this extension method exists, **use it in every controller action that needs the current user**. Never ask the client to send the userId.

### How to use it:

```csharp
// ✅ Correct — extract from token
[HttpGet]
[Authorize(Policy = "RequireUser")]
public async Task<IActionResult> GetMyCart()
{
    var userId = User.GetUserId(); // ← always from Claims
    var result = await _cartService.GetCartAsync(userId);
    return result.Success ? Ok(result) : BadRequest(result);
}
```

```csharp
// ❌ Wrong — never accept userId from request
[HttpGet]
public async Task<IActionResult> GetMyCart([FromQuery] string userId) // ← NEVER
```

```csharp
// ❌ Wrong — never accept userId in request body
public class GetCartRequest
{
    public string UserId { get; set; } // ← NEVER
}
```

**The extension reads:** `User.FindFirst(ClaimTypes.NameIdentifier)?.Value`

---

## 5. DTOs — Request & Response

**Location:** `ECommerce.Common/DTOs/`

If DTO classes already exist for a feature, **use them as-is**. If you're adding a new feature, follow the same folder structure:

```
ECommerce.Common/
└── DTOs/
    ├── Auth/
    │   ├── RegisterRequestDto.cs
    │   └── LoginResponseDto.cs
    ├── Products/
    │   ├── CreateProductDto.cs
    │   └── ProductResponseDto.cs
    └── Orders/
        ├── PlaceOrderDto.cs
        └── OrderResponseDto.cs
```

**Rules:**
- Request DTOs go in `*RequestDto` or `Create*Dto` / `Update*Dto`
- Response DTOs go in `*ResponseDto`
- Never expose entity classes directly from a controller — always map to a DTO
- Never add navigation properties to response DTOs unless they are needed by the client

---

## 6. Fluent Validation

**Location:** `ECommerce.BLL/Validators/` or `ECommerce.API/Validators/`

If validators exist, **add a validator for every new request DTO you create**. No controller action should do manual `if (string.IsNullOrEmpty(...))` checks.

### How to use it (follow the existing pattern):

```csharp
// ✅ Correct — one validator class per request DTO
public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");
    }
}
```

Since `AddFluentValidationAutoValidation()` is registered in `Program.cs`, validation runs **automatically** — you don't call the validator manually in the controller.

---

## 7. Policy-Based Authorization

**Location:** `ECommerce.API/Program.cs` (registered) + `[Authorize]` attributes on controllers

If policies are already registered, **use the exact policy names** — do not add new raw `[Authorize(Roles = "...")]` attributes.

```csharp
// ✅ Correct — use named policies
[Authorize(Policy = "RequireAdmin")]   // Admin only
[Authorize(Policy = "RequireUser")]    // Authenticated users (User + Admin)
[AllowAnonymous]                       // Public endpoints

// ❌ Wrong — never use raw role strings
[Authorize(Roles = "Admin")]  // ← avoid
```

**Policy definitions (already in Program.cs):**
- `RequireAdmin` → `RequireRole("Admin")`
- `RequireUser` → `RequireRole("User", "Admin")`

---

## 8. Soft Delete

**Location:** `AppDbContext.cs` (global query filter) + entity `IsDeleted` property

If `HasQueryFilter(e => !e.IsDeleted)` is configured on an entity, **never filter `IsDeleted` manually** in your queries — EF Core does it automatically.

```csharp
// ✅ Correct — EF Core filters deleted records automatically
var products = await _unitOfWork.Products.GetAllAsync();  // already excludes deleted

// ❌ Wrong — redundant and inconsistent
var products = await _context.Products
    .Where(p => !p.IsDeleted)  // ← don't do this, filter is already applied
    .ToListAsync();
```

To soft-delete an entity:
```csharp
// ✅ Correct
product.IsDeleted = true;
_unitOfWork.Products.Update(product);
await _unitOfWork.CommitAsync();

// ❌ Wrong — never hard delete entities that have soft-delete configured
_unitOfWork.Products.Delete(product);  // ← only for CartItems / OrderItems
```

---

## 9. Pagination — `PagedResult<T>`

**Location:** `ECommerce.Common/Wrappers/PagedResult.cs`

If this class exists, **all list endpoints that can return many records must use it**.



---

## 10. Using Mappster for mapping



---

## Project Structure Reference

```
ECommerce.sln
│
├── ECommerce.API/                    ← Presentation Layer
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── CategoriesController.cs
│   │   ├── ProductsController.cs
│   │   ├── CartController.cs
│   │   └── OrdersController.cs
│   ├── Extensions/
│   │   └── ClaimsPrincipalExtensions.cs
│   └── Program.cs
│
├── ECommerce.BLL/                    ← Business Logic Layer
│   ├── Services/
│   ├── Validators/
│   └── Mappings/MappingProfile.cs
│
├── ECommerce.DAL/                    ← Data Access Layer
│   ├── Entities/
│   ├── Repositories/
│   │   ├── Generic/
│   │   └── Specific/
│   ├── UnitOfWork/
│   └── AppDbContext.cs
│
└── ECommerce.Common/                 ← Shared Layer
    ├── DTOs/
    └── Wrappers/
        ├── ApiResponse.cs
        └── PagedResult.cs
```

---

## API Endpoints Quick Reference

| Method | Endpoint | Auth |
|--------|----------|------|
| POST | `/api/auth/register` | Anonymous |
| POST | `/api/auth/login` | Anonymous |
| GET | `/api/categories` | Anonymous |
| GET | `/api/categories/{id}` | Anonymous |
| POST | `/api/categories` | Admin |
| PUT | `/api/categories/{id}` | Admin |
| DELETE | `/api/categories/{id}` | Admin |
| POST | `/api/categories/{id}/image` | Admin |
| GET | `/api/products` | Anonymous |
| GET | `/api/products/{id}` | Anonymous |
| POST | `/api/products` | Admin |
| PUT | `/api/products/{id}` | Admin |
| DELETE | `/api/products/{id}` | Admin |
| POST | `/api/products/{id}/image` | Admin |
| GET | `/api/cart` | User |
| POST | `/api/cart` | User |
| PUT | `/api/cart` | User |
| DELETE | `/api/cart/{productId}` | User |
| POST | `/api/orders` | User |
| GET | `/api/orders` | User |
| GET | `/api/orders/{id}` | User |
| POST | `/api/image/upload` | Admin |

---

## Hard Rules — Non-Negotiable

1. **UserId comes from JWT Claims only.** Never from request body, query string, or route.
2. **Every service method returns `ApiResponse<T>`.** No raw returns, no thrown business exceptions.
3. **All writes go through `IUnitOfWork.CommitAsync()`.** Never call `DbContext.SaveChangesAsync()` in a service.
4. **Soft-deleted entities never appear in responses.** The global query filter handles this — don't add manual `.Where(!IsDeleted)`.
5. **One `CommitAsync()` per operation.** Especially for place-order — stock decrement, order creation, and cart clear are one atomic commit.
6. **Follow the folder structure.** New file goes in the same folder as its peers. No new top-level folders without a discussion.
7. **No business logic in controllers.** Controllers extract the userId, call one service method, and map the result to HTTP. That's it.
8. **No entity classes in API responses.** Always map to a DTO before returning from a controller.