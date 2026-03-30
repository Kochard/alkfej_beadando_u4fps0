# Measurement Results Application

## Overview

A full-stack measurement results management application with end-to-end deployment pipeline for storing and managing measurement data.

**Tech Stack:**
- **Frontend:** Angular
- **Backend:** ASP.NET Core Web API (.NET 10)
- **MCP Server:** ASP.NET Core service for model context protocol
- **Database:** MongoDB
- **Containerization:** Docker
- **CI/CD:** GitHub Actions
- **Container Registry:** GitHub Container Registry (GHCR)
- **Orchestration:** Kubernetes
- **Package Management:** Helm
- **GitOps:** Argo CD

---

## Features

- ✅ Create, read, update, and delete measurement results
- ✅ Paginated list view with filtering
- ✅ RESTful API endpoints
- ✅ Persistent storage with MongoDB
- ✅ Containerized deployment
- ✅ Automated CI/CD pipeline
- ✅ Kubernetes deployment with Helm charts
- ✅ **Microservice architecture with independent MCP component**
- ✅ **Advanced analytics and predictions via MCP microservice**
- ✅ MCP (Model Context Protocol) integration

---

## Architecture

```
Frontend (Angular + Nginx) → Main API (ASP.NET) → MongoDB
                              ↓
                       MCP Microservice (ASP.NET)
                              ↓
                           MongoDB (Shared)
```

- **Frontend:** Angular SPA served by Nginx
- **Main API:** Primary REST API for CRUD operations on measurement results
- **MCP Microservice:** Independent backend component providing analytics, insights, and predictions
- **Database:** Shared MongoDB instance for both services
- **Deployment:** Kubernetes with Argo CD for GitOps

---

## Repository Structure

```
├── backend-api/                 # .NET backend services
│   ├── CertificateStore.Api/    # Main Web API
│   └── CertificateStore.Mcp/    # MCP server
├── frontend/                    # Angular frontend
│   └── certificate-store-frontend/
├── k8s/                        # Kubernetes manifests
├── docs/                       # Documentation
│   ├── DEPLOYMENT-GUIDE.md
│   └── USER-GUIDE.md
├── docker-compose.yml          # Local development
├── mcp-server/                 # MCP server directory
└── requests/                   # API test requests
```

---

## Quick Start

### Prerequisites
- Docker & Docker Compose
- .NET 10 SDK
- Node.js 20+
- kubectl (for Kubernetes deployment)

### Local Development

1. **Clone the repository**
   ```bash
   git clone https://github.com/Kochard/alkfej_beadando_u4fps0.git
   cd alkfej_beadando_u4fps0
   ```

2. **Start services with Docker Compose**
   ```bash
   docker-compose up -d
   ```

3. **Access the application**
   - Frontend: http://localhost:4200
   - Backend API: http://localhost:5000
   - MongoDB: localhost:27017

### API Endpoints

- `GET /api/measurementresults` - List results (paginated)
- `POST /api/measurementresults` - Create new result
- `PUT /api/measurementresults/{id}` - Update result
- `DELETE /api/measurementresults/{id}` - Delete result

---

## Deployment

See [DEPLOYMENT-GUIDE.md](docs/DEPLOYMENT-GUIDE.md) for detailed Kubernetes deployment instructions.

## Usage

See [USER-GUIDE.md](docs/USER-GUIDE.md) for application usage instructions.

---

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run tests
5. Submit a pull request

---

## License

This project is licensed under the MIT License.

## Docker Images

Stored in GHCR:

- Backend  
  `ghcr.io/kochard/alkfej_beadando_u4fps0/backend:latest`

- Frontend  
  `ghcr.io/kochard/alkfej_beadando_u4fps0/frontend:latest`

---

## CI Pipeline

GitHub Actions:

- builds frontend and backend images
- pushes images to GHCR
- runs automatically on push to `main`

---

## Deployment (Kubernetes + Argo CD)

Application is deployed using:

- Kubernetes manifests (`k8s/`)
- Argo CD for automatic synchronization from Git

Deployment steps are described in:

docs/DEPLOYMENT-GUIDE.md

## Usage

After deployment:

Start port-forward:

kubectl port-forward service/frontend 8080:80

Open:

http://localhost:8080
Use the application:
create entries
edit entries
delete entries
browse paginated results

## Data Persistence

MongoDB uses a PersistentVolumeClaim:

data survives pod restart
verified in Kubernetes environment

## Notes

Designed for local Kubernetes (Docker Desktop)
No external ingress configured
Argo CD manages deployment from Git repository