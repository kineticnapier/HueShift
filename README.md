# HueShift

## Git remote setup
If `git pull` fails because no remote is configured for the current branch, add one and set the upstream tracking branch:

1. Check existing remotes:
   ```bash
   git remote -v
   ```
2. Add the remote if it is missing:
   ```bash
   git remote add origin <REMOTE_URL>
   ```
   Replace `<REMOTE_URL>` with the repository URL (for example `https://github.com/your-org/HueShift.git`).
3. If the remote exists but you need to update its URL, run:
   ```bash
   git remote set-url origin <REMOTE_URL>
   ```
4. Set the current branch (e.g., `work`) to track the remote branch:
   ```bash
   git push -u origin work
   # or, if the branch already exists remotely
   git branch --set-upstream-to=origin/work work
   ```
5. After configuring the remote, you can pull updates normally:
   ```bash
   git pull --rebase
   ```

### Container default
In this development container a local bare remote has been created and wired as `origin`:

- Remote path: `/workspace/remote/HueShift.git`
- Current branch tracking: `work` → `origin/work`

You can continue to pull from or push to this local remote. If you need to point to a different remote (for example GitHub), run `git remote set-url origin <REMOTE_URL>` and push with `-u` to update the upstream tracking branch.
