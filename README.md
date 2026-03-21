# Measurement Results Manager

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

Measurement Results Manager

Main planned operations:

- create measurement result
- list measurement results
- get measurement result by id
- update measurement result
- delete measurement result
- paginate measurement results

## Example business use

The application stores production or quality inspection measurement results.
Users can upload or manually enter a result, review it later, modify it, or remove it.

## Documentation

- User guide: docs/USER-GUIDE.md
- Deployment guide: docs/DEPLOYMENT-GUIDE.md