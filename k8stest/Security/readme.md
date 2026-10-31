
#Service Account
        When kubernetes workload needs to call api server
        they need an identity, i.e service account

        A ServiceAccount provides an identity for workloads running inside Kubernetes

        Every namespace gets a default ServiceAccount

        ServiceAccount
        │
        ├── Identity for Pods
        ├── Namespace-scoped
        ├── Does NOT grant permissions by itself
        └── Used together with RBAC

#RBAC

        Pod
 │
 │ uses
 ▼
ServiceAccount
 │
 │ RoleBinding
 ▼
Role
 │
 ├── resources
 └── verbs

# kubectl auth can-i delete pods #--as=system:serviceaccount:kubernetes-lab:app-reader -n kubernetes-lab