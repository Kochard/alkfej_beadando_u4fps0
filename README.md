# Certificate Store E2E Project

This project is a full end-to-end application and deployment solution built for coursework.

## Main technologies

- Frontend: Angular 21
- Backend API: ASP.NET 10 / C#
- Backend MCP Server: ASP.NET 10 / C#
- Database: MongoDB 8
- Containers: Docker
- CI: GitHub Actions
- Registry: GitHub Container Registry (ghcr.io)
- Orchestration: Kubernetes
- Database deployment: Helm
- CD: ArgoCD

## Planned modules

- frontend
- backend-api
- mcp-server
- docs
- k8s
- requests

## Main functional domain

X.509 Certificate Store

Main planned operations:

- create root certificate
- list root certificates
- delete root certificate
- store user certificate
- list user certificates
- delete user certificate
- sign uploaded CSR

## Documentation

- User guide: docs/USER-GUIDE.md
- Deployment guide: docs/DEPLOYMENT-GUIDE.md