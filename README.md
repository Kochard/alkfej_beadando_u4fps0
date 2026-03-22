# Measurement Results Application

## Overview

Full-stack application with end-to-end deployment pipeline.

Stack:

- Frontend: Angular
- Backend: ASP.NET Web API
- Database: MongoDB
- Containerization: Docker
- CI: GitHub Actions
- Registry: GitHub Container Registry (GHCR)
- Deployment: Kubernetes
- CD: Argo CD

---

## Features

- Create measurement results
- Edit measurement results
- Delete measurement results
- Paginated list view
- RESTful API
- Persistent storage (MongoDB)

---

## Architecture

Frontend → Nginx → Backend → MongoDB

- Frontend served by Nginx
- Nginx proxies `/api` requests to backend
- Backend communicates with MongoDB
- MongoDB uses PersistentVolumeClaim in Kubernetes

---

## Repository Structure

- `frontend/` → Angular app
- `backend-api/` → .NET API
- `k8s/` → Kubernetes manifests
- `docs/` → user and deployment guides

---

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