# Deployment Guide - Measurement Results Application

## 1. Overview

This project is deployed on a local Kubernetes cluster and uses:

- Frontend: Angular
- Backend: ASP.NET Web API
- Database: MongoDB
- Container images: GitHub Container Registry (GHCR)
- Continuous Integration: GitHub Actions
- Continuous Deployment: Argo CD

The application is deployed from Kubernetes manifests stored in the Git repository.

---

## 2. Prerequisites

Before deployment, install and prepare the following:

- Docker Desktop
- Kubernetes enabled in Docker Desktop
- kubectl
- Git
- Access to the GitHub repository

Optional but recommended:
- GitHub account with access to GHCR images

---

## 3. Repository Structure

Relevant folders and files:

- `k8s/` → Kubernetes manifests
- `k8s/backend-deployment.yaml`
- `k8s/frontend-deployment.yaml`
- `k8s/mongo-deployment.yaml`
- `k8s/mongo-pvc.yaml`
- `k8s/argocd-application.yaml`
- `docs/USER-GUIDE.md`
- `docs/DEPLOYMENT-GUIDE.md`

---

## 4. Container Images

This project uses container images stored in GHCR.

Images:

- Backend  
  `ghcr.io/kochard/alkfej_beadando_u4fps0/backend:latest`

- Frontend  
  `ghcr.io/kochard/alkfej_beadando_u4fps0/frontend:latest`

MongoDB uses the official image:

- `mongo:8`

---

## 5. Continuous Integration

GitHub Actions automatically:

- builds backend image
- builds frontend image
- pushes both images to GHCR

The CI pipeline is triggered on push to the `main` branch.

---

## 6. Kubernetes Deployment Files

The application is deployed with these Kubernetes resources:

- MongoDB Deployment + Service
- MongoDB PersistentVolumeClaim
- Backend Deployment + Service
- Frontend Deployment + Service
- Argo CD Application manifest

MongoDB persistence is provided by:

- `mongo-pvc.yaml`

This ensures MongoDB data survives pod restart.

---

## 7. Install Argo CD

Create the Argo CD namespace:

kubectl create namespace argocd
Install Argo CD:

kubectl apply -n argocd --server-side --force-conflicts -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml

Check pods:

kubectl get pods -n argocd

Wait until Argo CD pods are running.

## 8. Access Argo CD

Port-forward the Argo CD server:

kubectl port-forward svc/argocd-server -n argocd 8090:443

Open in browser:

https://localhost:8090

Because this is a local installation, the browser may show a certificate warning.

## 9. Get Argo CD Admin Password

Read the initial admin password secret:

kubectl get secret argocd-initial-admin-secret -n argocd -o jsonpath="{.data.password}"

Decode it with PowerShell:

powershell -command "[System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String((kubectl get secret argocd-initial-admin-secret -n argocd -o jsonpath='{.data.password}')))"

Login details:

Username: admin
Password: decoded value

## 10. Deploy the Application with Argo CD

Apply the Argo CD Application manifest:

kubectl apply -f k8s/argocd-application.yaml

Check the application:

kubectl get applications -n argocd

The application should become:

Synced
Healthy

Argo CD will deploy the Kubernetes manifests from the Git repository automatically.

## 11. Access the Frontend

Port-forward the frontend service:

kubectl port-forward service/frontend 8080:80

Open in browser:

http://localhost:8080

The application should load and allow:

creating measurement results
editing results
deleting results
navigating paginated results

## 12. Verify Backend Access Through Frontend

The frontend uses nginx proxy configuration to forward:

/api/...

to the backend service inside Kubernetes.

This avoids browser CORS issues.

No separate backend browser access is required for normal usage.

## 13. Verify MongoDB Persistence

MongoDB uses a PersistentVolumeClaim:

mongo-pvc

To verify persistence:

Create a new measurement result

Delete the MongoDB pod:

kubectl delete pod -l app=mongo
Wait for Kubernetes to recreate the pod
Refresh the frontend
Verify that the created data is still present

This confirms persistent storage is working.

## 14. Useful Verification Commands

Check pods:

kubectl get pods

Check services:

kubectl get services

Check PVC:

kubectl get pvc

Check Argo CD applications:

kubectl get applications -n argocd

Check backend logs:

kubectl logs deployment/backend

Check frontend logs:

kubectl logs deployment/frontend

Check MongoDB logs:

kubectl logs deployment/mongo

## 15. Troubleshooting
Frontend opens but no data appears
verify backend pod is running
verify frontend nginx proxy is configured correctly
verify MongoDB pod is running
Argo CD application not syncing
check kubectl get applications -n argocd
inspect Argo CD UI for manifest errors
Data disappears after Mongo restart
verify mongo-pvc exists
verify PVC status is Bound
Frontend not accessible

restart port-forward:

kubectl port-forward service/frontend 8080:80

## 16. Notes

This deployment guide is for a local Kubernetes environment.

For production deployment, additional components would typically be needed, such as:

Ingress
DNS / domain name
TLS certificates
secret management
production-grade storage class