# netcore31HelloWorld

Asp.net Core 3.1 Hello World

## Demo 登录（勿用于生产）

本仓库含 **演示用** Cookie 登录，凭据写在 `appsettings.json` 的 `DemoAuth` 节点（明文比较）：

| 项 | 值 |
|---|---|
| 用户名 | `admin` |
| 密码 | `admin123` |

**仅供本地 / 测试演示，切勿用于生产环境。**

- 登录页：`/Account/Login`
- 登出：`/Account/Logout`
- 受保护：全部 Razor Pages（除 Login / Error）、静态页 `/time.html`
- 公开：`GET /api/time`（时间页脚本调用）

未登录访问受保护资源会跳转到登录页。
