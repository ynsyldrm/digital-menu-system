# 🍽️ Digital Menu & Order System

A microservices-based restaurant ordering system built with .NET 9, Next.js, and modern architectural patterns.

## 🏗️ Architecture

### Services
- **Menu Service** - Manages menu items and categories
- **Order Service** - Handles order creation, status tracking, and saga orchestration
- **Kitchen Service** - Processes orders and manages preparation status

### Tech Stack
- **Backend**: .NET 9, Onion Architecture, CQRS, MediatR, MassTransit, OutBox Pattern, Saga Pattern
- **Frontend**: Next.js 14, TypeScript, TanStack Query, Tailwind CSS
- **Database**: PostgreSQL
- **Message Broker**: RabbitMQ
- **DevOps**: Docker, Kubernetes, GitHub Actions

## 🚀 Quick Start

### Prerequisites
- .NET 9 SDK
- Node.js 20+
- Docker & Docker Compose
- PostgreSQL (or use Docker)

### Running the Project

1. **Clone and setup infrastructure:**
```bash
# Start databases and message broker
cd DigitalMenuSystem/infrastructure/docker
docker-compose up -d
```

2. **Run Menu Service:**
```bash
cd DigitalMenuSystem/services/MenuService/MenuService
dotnet run
```

3. **Run Order Service:**
```bash
cd DigitalMenuSystem/services/OrderService/OrderService
dotnet run
```

4. **Run Kitchen Service:**
```bash
cd DigitalMenuSystem/services/KitchenService/KitchenService
dotnet run
```

5. **Run Frontend:**
```bash
cd DigitalMenuSystem/frontend
npm install
npm run dev
```

### API Endpoints

#### Menu Service (http://localhost:5001)
- `GET /api/menu-items` - Get all menu items
- `POST /api/menu-items` - Create new menu item

#### Order Service (http://localhost:5002)
- `POST /api/orders` - Create new order
- `GET /api/orders/{id}` - Get order details

#### Kitchen Service (http://localhost:5003)
- `PUT /api/orders/{id}/status` - Update order status

#### Frontend (http://localhost:3000)
- Interactive menu and ordering interface

## 📁 Project Structure

```
DigitalMenuSystem/
├── services/
│   ├── MenuService/          # Menu management microservice
│   ├── OrderService/         # Order processing microservice
│   └── KitchenService/       # Kitchen operations microservice
├── frontend/                 # Next.js React application
├── infrastructure/
│   ├── docker/              # Docker configurations
│   └── k8s/                 # Kubernetes manifests
└── .github/workflows/       # CI/CD pipelines
```

## 🔧 Development

### Adding New Features
1. Follow CQRS pattern for commands and queries
2. Use MediatR for request handling
3. Implement domain events for cross-service communication
4. Add OutBox pattern for reliable messaging
5. Use Saga pattern for distributed transactions

### Database Migrations
```bash
# Menu Service
cd services/MenuService/MenuService
dotnet ef database update

# Order Service
cd services/OrderService/OrderService
dotnet ef database update
```

## 🧪 Testing

```bash
# Run tests for all services
dotnet test

# Run frontend tests
cd frontend
npm test
```

## 📦 Deployment

### Docker Build
```bash
# Build all services
docker-compose -f infrastructure/docker/docker-compose.yml build

# Deploy to Kubernetes
kubectl apply -f infrastructure/k8s/
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📄 License

This project is licensed under the MIT License.