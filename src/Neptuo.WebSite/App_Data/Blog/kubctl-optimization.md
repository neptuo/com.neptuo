There are few tweaks that I found useful when working with [kubectl](https://kubernetes.io/docs/reference/kubectl/) to manage kubernetes cluster.

> I'm running a cluster of Raspberry Pi.

Kubectl lets you manage your cluster from command line. By default it requires a lots of typing. A lot! When you need to use it on daily basis, these tweaks can save your fingers.

> All of the tweaks are set in user's `.bashrc`. At the end there are all changes at once.

## Kubectl alias

Instead of typing `kubectl` everytime (also note that when you have both `kubectl` and `kubeadm`, bash's tab completion doesn't help you a lot), I'm using simple `k`.

```bash
alias k=kubectl
```

## Kubectl completion

This one helps you complete `kubectl` sub commands and parameters. It also supports completion of pod/svc/deploy/etc names. So if you have for example a single pod managed by replica set and deployment, it may be named as `webcam-69d6fdd46c-kbvfk`. The deployment itself is named `webcam`, replica set `69d6fdd46c` and pod has the suffix `kbvfk`. When you need to get logs of the pod, you need to type the whole name. And this name is changed everytime the pod dies. With completion, you can just type `k logs webcam`, press `tab` and rest of the name will be populated.

```bash
source <(kubectl completion bash)
complete -F __start_kubectl kubectl k
```

The end of the second line (`kubectl k`) defines commands for which the completion should be used (`kubectl` and `k` in this case).

> If your pod is in a different namespace than the default one, type the namespace before pod name, like `k logs -n monitoring webcam` and than press the `tab`.

## Changing default namespace

By default, kubectl "default" namespace is stored in `.kube/config`, and whenever you are using `kubectl` for different namespace, you need to include `-n` or `--namespace` in your command. It's quite hard to change the default namespace in `.kube/config`, fortunately there is a [tool to the rescue - kubens and kubectx](https://github.com/ahmetb/kubectx).

It shows you all namespaces and contexts and lets change the defaults.

## Kubens and kubectx alias

`kubens` and `kubectx` is still quite verbose, so aliases can help.

```bash
alias kns=kubens
alias kctx=kubectx
```

## Kubens and kubectx completion

As with `kubectl`, even `kubens`/`kns` and `kubectx`/`kctx` supports completion.

```bash
_kube_namespaces()
{
  local curr_arg;
  curr_arg=${COMP_WORDS[COMP_CWORD]}
  COMPREPLY=( $(compgen -W "- $(kubectl get namespaces -o=jsonpath='{range .items[*].metadata.name}{@}{"\n"}{end}')" -- $curr_arg ) );
}
complete -F _kube_namespaces kubens kns
```

```bash
_kube_contexts()
{
  local curr_arg;
  curr_arg=${COMP_WORDS[COMP_CWORD]}
  COMPREPLY=( $(compgen -W "- $(kubectl config get-contexts --output='name')" -- $curr_arg ) );
}
alias kctx=kubectx
complete -F _kube_contexts kubectx kctx
```

## Summary
- `kubectl`/`k` alias for `kubectl`.
- `k` completion.
- `kubens` for changing default `.kube/config` namespace.
- `kubectx` for changing `.kube/config` context.
- `kns` alias for `kubens`.
- `kctx` alias for `kubectx`.
- `kubens`/`kns` completion.
- `kubectx`/`kctx` completion.

### Changes to .bashrc at once

```bash
# kubectl
alias k=kubectl
source <(kubectl completion bash)
complete -F __start_kubectl kubectl k

# kubens
_kube_namespaces()
{
  local curr_arg;
  curr_arg=${COMP_WORDS[COMP_CWORD]}
  COMPREPLY=( $(compgen -W "- $(kubectl get namespaces -o=jsonpath='{range .items[*].metadata.name}{@}{"\n"}{end}')" -- $curr_arg ) );
}
alias kns=kubens
complete -F _kube_namespaces kubens kns

# kubectx
_kube_contexts()
{
  local curr_arg;
  curr_arg=${COMP_WORDS[COMP_CWORD]}
  COMPREPLY=( $(compgen -W "- $(kubectl config get-contexts --output='name')" -- $curr_arg ) );
}
alias kctx=kubectx
complete -F _kube_contexts kubectx kctx
```