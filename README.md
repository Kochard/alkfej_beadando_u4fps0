# Full-Stack Measurement Results Application

## Overview

This project is a full-stack application with:

- Frontend: Angular
- Backend: .NET Web API
- Database: MongoDB
- Containerization: Docker
- Deployment: Kubernetes
- CI/CD: GitHub Actions + GHCR

---

## Requirements

Install:

- Docker Desktop (with Kubernetes enabled)
- Git
- Node.js (only if running locally without Docker)

---

## Clone Repository

```bash
git clone https://github.com/Kochard/alkfej_beadando_u4fps0.git
cd alkfej_beadando_u4fps0
Run with Kubernetes
1. Enable Kubernetes

In Docker Desktop:

Settings → Kubernetes → Enable Kubernetes
2. Apply manifests
kubectl apply -f k8s/
3. Verify pods
kubectl get pods

All should be:

Running
4. Access application
kubectl port-forward service/frontend 8080:80

Open:

http://localhost:8080
Data Persistence

MongoDB uses a PersistentVolumeClaim:

mongo-pvc

Data survives:

pod restart
deployment restart
Docker Images

Images are hosted on GitHub Container Registry:

Backend:

ghcr.io/kochard/alkfej_beadando_u4fps0/backend:latest

Frontend:

ghcr.io/kochard/alkfej_beadando_u4fps0/frontend:latest
CI/CD

GitHub Actions automatically:

builds Docker images
pushes to GHCR

Triggered on push to main.

Architecture

Frontend → Nginx → Backend → MongoDB

Frontend calls /api/...
Nginx proxies to backend service
Backend communicates with MongoDB
Notes
No external access (local Kubernetes only)
For production, use Ingress + domain