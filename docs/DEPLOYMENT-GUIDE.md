# Deployment Guide - Measurement Results System

## Table of Contents

1. [Overview](#overview)
2. [Prerequisites](#prerequisites)
3. [Architecture](#architecture)
4. [Quick Start (Docker Compose)](#quick-start-docker-compose)
5. [Kubernetes Deployment (Local)](#kubernetes-deployment-local)
6. [Production Deployment (Argo CD)](#production-deployment-argo-cd)
7. [Advanced Configuration](#advanced-configuration)
8. [Monitoring & Troubleshooting](#monitoring--troubleshooting)

---

## Overview

This project is a full-stack containerized system for managing measurement results with an end-to-end CI/CD pipeline:

- **Frontend**: Angular 21
- **Backend**: ASP.NET 10 Web API
- **Database**: MongoDB 8
- **Registry**: GitHub Container Registry (GHCR)
- **CI**: GitHub Actions
- **Deployment**: Kubernetes
- **CD**: Argo CD

### System Components

- **frontend** - Angular application served via Nginx
- **backend** - ASP.NET Core REST API
- **mcp** - Additional backend service  
- **mongo** - MongoDB database with persistent storage

### Repository Structure

```
alkfej_beadando_u4fps0/
├── frontend/certificate-store-frontend/    # Angular frontend
├── backend-api/
│   ├── CertificateStore.Api/               # Main backend API
│   └── CertificateStore.Mcp/               # MCP service
├── k8s/                                     # Kubernetes manifests
├── docker-compose.yml                       # Local development setup
├── docs/                                    # Documentation
└── .github/workflows/                       # CI/CD pipeline
```

---

## Prerequisites

### Required Software

- **Git** - Version control
- **Docker Desktop** - Container runtime
- **kubectl** - Kubernetes CLI tool
- **Kubernetes** - Enabled in Docker Desktop (for local K8s deployment)
- **Helm** (optional) - Package manager for Kubernetes

### Installation Instructions

**Windows (using PowerShell or Command Prompt):**

1. Install Docker Desktop from https://www.docker.com/products/docker-desktop/
2. Enable Kubernetes in Docker Desktop Settings:
   - Right-click Docker icon → Settings
   - Navigate to Kubernetes tab
   - Check "Enable Kubernetes"
   - Click "Apply & Restart"
3. Verify installation:

```bash
docker --version
kubectl version --client
```

### Account Access

- **GitHub Account** - Access to the repository (for CI/CD)
- **GHCR Access** - Already configured if images are auto-built

---

## Architecture

### Network Topology

```
┌─────────────────────────────────────────┐
│           USER BROWSER                   │
└──────────────┬──────────────────────────┘
               │ HTTP
┌──────────────▼──────────────────────────┐
│      NGINX (Frontend)                    │
│  Port 80 (or 30007 in K8s)               │
└──────────────┬──────────────────────────┘
               │ /api requests proxied
┌──────────────▼──────────────────────────┐
│      ASP.NET Backend API                 │
│  Port 8080 → 5202 (Docker Compose)      │
└──────────────┬──────────────────────────┘
               │ queries
┌──────────────▼──────────────────────────┐
│      MongoDB Database                    │
│  Port 27017                              │
└──────────────────────────────────────────┘
```

### Container Images

| Service  | Image | Source |
|----------|-------|--------|
| Backend  | `ghcr.io/kochard/alkfej_beadando_u4fps0/backend:latest` | GHCR |
| Frontend | `ghcr.io/kochard/alkfej_beadando_u4fps0/frontend:latest` | GHCR |
| MCP      | `ghcr.io/kochard/alkfej_beadando_u4fps0/mcp:latest` | GHCR |
| MongoDB  | `mongo:8.0` | Docker Hub |

---

## Quick Start (Docker Compose)

### What is Docker Compose?

Docker Compose allows you to run all services locally with a single command. Perfect for development and testing.

### Step 1: Clone Repository

```bash
cd C:\Users\kovac
git clone https://github.com/Kochard/alkfej_beadando_u4fps0.git
cd alkfej_beadando_u4fps0
```

### Step 2: Start Docker Desktop

1. Open Docker Desktop application
2. Wait for it to fully load (check the system tray icon)

### Step 3: Launch Application

```bash
docker compose up -d
```

This will:
- Pull MongoDB image
- Build and run frontend service
- Build and run backend service
- Create persistent volume for database

### Step 4: Access Application

Open your browser and navigate to:

```
http://localhost:4200
```

### Service Endpoints

| Service | Port | URL |
|---------|------|-----|
| Frontend | 4200 | http://localhost:4200 |
| Backend API | 5202 | http://localhost:5202 |
| MongoDB | 27017 | localhost:27017 |

### Stop Application

```bash
docker compose down
```

To also remove database data:

```bash
docker compose down -v
```

---

## Kubernetes Deployment (Local)

### What is Kubernetes?

Kubernetes is a container orchestration platform. Local deployment is ideal for staging and testing production-like environments.

### Step 1: Verify Kubernetes is Running

```bash
kubectl cluster-info
```

You should see output indicating the cluster is accessible.

### Step 2: Create Namespace (Optional but Recommended)

```bash
kubectl create namespace measurement-results
```

To use this namespace by default:

```bash
kubectl config set-context --current --namespace=measurement-results
```

### Step 3: Deploy MongoDB Persistent Volume

```bash
kubectl apply -f k8s/mongo-pvc.yaml
```

Verify the PVC is created:

```bash
kubectl get pvc
```

### Step 4: Deploy MongoDB

```bash
kubectl apply -f k8s/mongo-deployment.yaml
```

Check MongoDB pod status:

```bash
kubectl get pods -l app=mongo
kubectl logs -l app=mongo
```

Wait for MongoDB to be ready (STATUS = Running):

```bash
kubectl get pods -w -l app=mongo
```

### Step 5: Deploy Backend API

```bash
kubectl apply -f k8s/backend-deployment.yaml
```

Check backend pod status:

```bash
kubectl get pods -l app=backend
kubectl logs -l app=backend
```

### Step 6: Deploy Frontend

```bash
kubectl apply -f k8s/frontend-deployment.yaml
```

Check frontend pod status:

```bash
kubectl get pods -l app=frontend
```

### Step 7: Verify All Services

```bash
kubectl get pods
kubectl get services
kubectl get deployments
```

### Step 8: Access Application

#### Option A: Port Forwarding

```bash
kubectl port-forward service/frontend 8080:80
```

Open: http://localhost:8080

#### Option B: NodePort Service

The frontend service is configured as NodePort (port 30007), accessible at:

```
http://localhost:30007
```

### Deploy MCP Service (Optional)

```bash
kubectl apply -f k8s/mcp-deployment.yaml
```

### View All Resources

```bash
# View all resources
kubectl get all

# View detailed information
kubectl describe pod <pod-name>
kubectl describe service <service-name>
kubectl describe deployment <deployment-name>
```

---

## Helm Deployment (Local)

### What is Helm?

Helm is a package manager for Kubernetes that simplifies deployment and management of applications. It uses charts (packages) with templated YAML files for customizable, versioned deployments.

### Prerequisites

- Helm installed: `choco install kubernetes-helm` (Windows) or download from https://helm.sh/
- Kubernetes cluster running (Docker Desktop or other)

### Step 1: Install Helm

Verify Helm installation:

```bash
helm version
```

### Step 2: Deploy with Helm

Navigate to the k8s directory:

```bash
cd k8s
```

Update dependencies (for MongoDB subchart):

```bash
helm dependency update
```

Install the chart:

```bash
helm install measurement-system .
```

Or with a custom release name:

```bash
helm install my-release .
```

### Step 3: Verify Deployment

Check all resources:

```bash
helm list
kubectl get all
```

Check pod status:

```bash
kubectl get pods
kubectl logs -l app.kubernetes.io/name=measurement-results-system
```

### Step 4: Access Application

Port-forward the frontend service:

```bash
kubectl port-forward service/my-release-measurement-results-system-frontend 8080:80
```

Open: http://localhost:8080

### Customization

Override values during installation:

```bash
helm install my-release . --set frontend.replicas=2 --set mongodb.persistence.size=16Gi
```

Or create a custom values file:

```bash
helm install my-release . -f my-values.yaml
```

### Upgrading

Upgrade the deployment:

```bash
helm upgrade my-release .
```

### Uninstalling

Remove the deployment:

```bash
helm uninstall my-release
```

### Chart Structure

```
k8s/
├── Chart.yaml          # Chart metadata and dependencies
├── values.yaml         # Default configuration values
├── templates/          # Kubernetes manifest templates
│   ├── _helpers.tpl    # Template helpers
│   ├── frontend.yaml   # Frontend deployment/service
│   ├── backend.yaml    # Backend deployment/service
│   └── mcp.yaml        # MCP deployment/service
└── charts/             # Subcharts
    └── mongodb/        # MongoDB subchart
```

---

## Production Deployment (Argo CD)

### What is Argo CD?

Argo CD is a declarative GitOps continuous delivery tool that automatically synchronizes your Kubernetes cluster with your Git repository.

### Prerequisites

- Access to a Kubernetes cluster (cloud provider or on-premises)
- kubectl access to the cluster
- GitHub repository access

### Step 1: Install Argo CD

```bash
kubectl create namespace argocd
kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml
```

Wait for all Argo CD components to be ready:

```bash
kubectl wait --for=condition=Ready pod -l app.kubernetes.io/name=argocd-server -n argocd --timeout=300s
```

### Step 2: Access Argo CD UI

Get the initial admin password:

```bash
kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}" | base64 -d
```

Port-forward to Argo CD server:

```bash
kubectl port-forward svc/argocd-server -n argocd 8080:443
```

Open: https://localhost:8080

Log in with:
- Username: `admin`
- Password: (from previous command)

### Step 3: Create Argo CD Application

Apply the Argo CD application manifest:

```bash
kubectl apply -f k8s/argocd-application.yaml
```

### Step 4: Verify Application Sync

In Argo CD UI:

1. Navigate to Applications
2. Click on "measurement-results-app"
3. Verify all components are "Synced"
4. Check the application topology

### Step 5: Deploy Application

The application will auto-sync based on the Git repository. To manually trigger sync:

```bash
argocd app sync measurement-results-app
```

### How It Works

1. **Git as Source of Truth**: All Kubernetes manifests are stored in git (`k8s/` folder)
2. **Automatic Sync**: Argo CD continuously monitors the repository and automatically applies changes
3. **Self-Healing**: If cluster state drifts from Git, Argo CD automatically corrects it
4. **Pruning**: Deleted resources in Git are automatically removed from the cluster

### Access Application

```bash
kubectl port-forward service/frontend 8080:80
```

Open: http://localhost:8080

---

## Advanced Configuration

### Environment Variables

#### Backend Configuration

Located in `backend-api/CertificateStore.Api/appsettings.json`:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://mongo:27017"
  }
}
```

For Kubernetes deployments, modify the Deployment manifest to pass environment variables:

```yaml
env:
  - name: MongoDbSettings__ConnectionString
    value: "mongodb://mongo-mongodb:27017"
```

### Database Backup

#### Using MongoDB Tools

```bash
# Export data
kubectl exec -it <mongo-pod-name> -- mongodump --out /tmp/backup

# Copy from pod
kubectl cp <mongo-pod-name>:/tmp/backup ./local-backup
```

#### Using Persistent Volume Snapshots

In production, use cloud provider snapshots:
- **AWS**: EBS Snapshots
- **Azure**: Managed Disk Snapshots
- **GCP**: Persistent Disk Snapshots

### Scaling

#### Scale Backend Replicas

```bash
kubectl scale deployment backend --replicas=3
```

#### Scale Frontend Replicas

```bash
kubectl scale deployment frontend --replicas=2
```

### Resource Limits

Edit deployment manifests to add resource requests and limits:

```yaml
resources:
  requests:
    memory: "256Mi"
    cpu: "250m"
  limits:
    memory: "512Mi"
    cpu: "500m"
```

---

## CI/CD Pipeline

### GitHub Actions Workflow

Defined in `.github/workflows/docker.yml`:

**Triggers**: Automatically runs on push to `main` branch

**Steps**:
1. Checkout repository
2. Log in to GitHub Container Registry (GHCR)
3. Build backend image
4. Push backend image to GHCR
5. Build frontend image
6. Push frontend image to GHCR

### Image Naming Convention

Images are tagged with:
- `latest` - Current production version
- `<git-commit-sha>` - Specific version

### Manual Image Builds

Build locally and push to GHCR:

```bash
# Backend
docker build -t ghcr.io/kochard/alkfej_beadando_u4fps0/backend:latest ./backend-api/CertificateStore.Api
docker push ghcr.io/kochard/alkfej_beadando_u4fps0/backend:latest

# Frontend
docker build -t ghcr.io/kochard/alkfej_beadando_u4fps0/frontend:latest ./frontend/certificate-store-frontend
docker push ghcr.io/kochard/alkfej_beadando_u4fps0/frontend:latest
```

---

## Monitoring & Troubleshooting

### Check Pod Status

```bash
# All pods
kubectl get pods

# Specific pod logs
kubectl logs <pod-name>

# Real-time logs
kubectl logs -f <pod-name>

# Previous pod logs (if crashed)
kubectl logs <pod-name> --previous
```

### Check Services

```bash
# All services
kubectl get services

# Service details
kubectl describe service <service-name>

# Service endpoints
kubectl get endpoints <service-name>
```

### Common Issues & Solutions

#### Frontend Not Loading

**Problem**: 404 error or blank page

**Solutions**:
1. Check frontend pod is running: `kubectl get pods -l app=frontend`
2. Check logs: `kubectl logs -l app=frontend`
3. Verify service is exposed: `kubectl get svc frontend`
4. Port-forward and retry: `kubectl port-forward svc/frontend 8080:80`

#### Backend API Connection Failed

**Problem**: API errors or network timeouts

**Solutions**:
1. Check backend pod: `kubectl get pods -l app=backend`
2. Check MongoDB connection in backend logs: `kubectl logs -l app=backend`
3. Verify MongoDB is running: `kubectl get pods -l app=mongo`
4. Check network connectivity: `kubectl exec -it <backend-pod> -- ping mongo`

#### MongoDB Not Available

**Problem**: Database connection errors

**Solutions**:
1. Check MongoDB pod: `kubectl get pods -l app=mongo`
2. Check MongoDB logs: `kubectl logs -l app=mongo`
3. Verify PVC is bound: `kubectl get pvc`
4. Check disk space: `kubectl describe pvc mongo-pvc`

#### Argo CD Out of Sync

**Problem**: Argo CD shows "OutOfSync" status

**Solutions**:
1. Check Git repository for uncommitted changes
2. Force sync: `argocd app sync measurement-results-app --force`
3. Hard refresh: `argocd app sync measurement-results-app --hard-refresh`

### Useful Debug Commands

```bash
# Describe pod for events
kubectl describe pod <pod-name>

# Execute command in pod
kubectl exec -it <pod-name> -- /bin/bash

# Port-forward for direct access
kubectl port-forward pod/<pod-name> 8080:8080

# View resource usage
kubectl top pods
kubectl top nodes

# View events
kubectl get events --sort-by='.lastTimestamp'

# Run a debug pod
kubectl run -it debug -- /bin/bash --image=ubuntu:latest
```

### Health Checks

To verify all components are healthy:

```bash
# Frontend health
curl http://localhost:4200/

# Backend health (add health endpoint if available)
curl http://localhost:5202/api/health

# MongoDB health
kubectl exec -it <mongo-pod> -- mongosh --eval "db.adminCommand('ping')"
```

---

## Cleanup

### Remove Local Deployment (Docker Compose)

```bash
docker compose down -v  # -v removes volumes too
```

### Remove Kubernetes Deployment

```bash
# Remove all manifests
kubectl delete -f k8s/

# Remove namespace (if created)
kubectl delete namespace measurement-results
```

### Remove Argo CD

```bash
kubectl delete namespace argocd
```

---

## Summary

| Environment | Command | Scale |
|---|---|---|
| **Development** | `docker compose up -d` | Single machine |
| **Local K8s** | `kubectl apply -f k8s/` | Local Kubernetes |
| **Production** | `kubectl apply -f k8s/argocd-application.yaml` | Cloud/On-prem K8s |

For questions or issues, refer to the component documentation or logs using `kubectl logs`.