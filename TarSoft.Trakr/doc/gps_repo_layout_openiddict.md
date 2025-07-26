# 📁 GPS Tracking Platform – Repository Layout (OpenIddict, .NET 9)

This layout supports a modular, scalable system including IoT, real-time telemetry, AI, and multi-frontend support.

---

## 📁 Root Structure

```
/gps-tracking-platform/
├── src/
│   ├── Services/
│   │   ├── TelemetryIngestor/           # ASP.NET Core Minimal API or MQTT handler
│   │   ├── TelemetryProcessor/          # .NET 9 Worker Service
│   │   ├── AlertEngine/                 # AI/ML rule evaluation
│   │   ├── Provisioning/                # Device onboarding, QR claiming
│   │   ├── AuthServer/                  # OpenIddict-based Identity/OAuth2 server
│   │   └── ApiGateway/                  # YARP reverse proxy or API gateway
│   ├── Frontends/
│   │   ├── WebApp/                      # Blazor WASM or React app
│   │   └── MobileApp/                   # MAUI or Flutter client
│   ├── Shared/
│   │   ├── Contracts/                   # DTOs, Protobufs, shared API models
│   │   ├── Infrastructure/              # Common EF Core, Redis, helpers
│   │   └── Identity/                    # JWT claims, tenant resolution, user roles
├── tests/
│   ├── TelemetryProcessor.Tests/
│   ├── AlertEngine.Tests/
│   ├── Shared.Tests/
│   └── Integration/
│       └── EndToEnd/
├── devops/
│   ├── docker/                          # Dockerfiles, docker-compose setups
│   ├── github-actions/                  # CI/CD workflow definitions
│   └── terraform/                       # Infrastructure as Code
├── docs/
│   ├── architecture.md
│   ├── prd.md
│   └── delivery-plan.md
├── .github/
│   └── ISSUE_TEMPLATE.md
├── README.md
└── LICENSE
```

---

## 🔐 Identity with OpenIddict

- Full control of the identity system inside your own ASP.NET Core API
- Works with ASP.NET Core Identity for user/role management
- Supports OAuth2 flows: password, client credentials, refresh tokens
- Issues JWT access/refresh tokens
- Database-backed via EF Core

---

## ✅ Benefits

- Clear separation of services and frontends
- Fully open-source auth stack (no Duende license)
- Flexible for IoT/device provisioning and multi-tenant flows
- Clean structure for scaling and CI/CD setup
