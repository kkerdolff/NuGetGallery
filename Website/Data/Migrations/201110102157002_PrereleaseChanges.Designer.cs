using System.Data.Entity.Migrations.Infrastructure;

namespace NuGetGallery.Data.Migrations
{
    public partial class PrereleaseChanges : IMigrationMetadata
    {
        string IMigrationMetadata.Id
        {
            get { return "201110102157002_PrereleaseChanges"; }
        }

        string IMigrationMetadata.Source
        {
            get { return null; }
        }

        string IMigrationMetadata.Target
        {
            get { return "H4sIAAAAAAAEAOy9B2AcSZYlJi9tynt/SvVK1+B0oQiAYBMk2JBAEOzBiM3mkuwdaUcjKasqgcplVmVdZhZAzO2dvPfee++999577733ujudTif33/8/XGZkAWz2zkrayZ4hgKrIHz9+fB8/Iv7Hv/cffPx7vFuU6WVeN0W1/Oyj3fHOR2m+nFazYnnx2Ufr9nz74KPf4+g3Th6fzhbv0p807fbQjt5cNp99NG/b1aO7d5vpPF9kzXhRTOuqqc7b8bRa3M1m1d29nZ2Du7s7d3MC8RHBStPHr9bLtljk/Af9eVItp/mqXWflF9UsLxv9nL55zVDTF9kib1bZNP/soxfrz/P286ws8/r6o/S4LDLC4XVenr8nQjsPgdBHtivq7JSQaq/fXK9y7vCzj75q8tpvQW1+r/w6+IA+ellXq7xur1/l5/oeNfoovRu+eLf7pn3Pfwmdf/bR2bK9t/dR+mJdltmkpA/Os7LJP0pXnz563VZ1/nm+zOuszWcvs7bNa5qQs1nOyCsRHq0+vR0dHt7d2QMd7mbLZdVmLc1uD/MOnserwkP183Uxi2C6GcTpIivK49mszpvGAHrd1sRzH6XPinf57Hm+vGjnFtoX2TvzCf36UfrVsiAWpZfaep37vcvfmzv/ith7eV7Ui3z2c4rHt7Nmjhlsmquqnv3QuwdvL+m3H3rHQvSyrK5yO+onVVXm2fLrMdKJTCcz75vqbb78oQ/JTOKrvMnb/7egcPpuVdRMlKekKgxC+P1NgWm/Af6L7LK44Nc7PX1BwpJd5CQwr/KSGzTzYiU6eAyu+v1di2d1tXhVoY/gi9//dbWup8Cpin37Jqsv8vb2GAHGBnT06w4u/GkcEfkqhsXju85GbLQczJg6oB9ZkA6eT6qZRfSHJiCYfszuJiLdBs5rItOHqq3X68lP51ML5odGgzfVTRSIoz8oeYaoUeHzZeD3dy2dHEYb9EQy3up9dYQM/UN0lukxrrMM1l9LWwDgj7REB0/8+7MvIYMMg+n9mkYlyimhvflabPIym74lZnuVXxRNK6b9R1zTwfPsh+9JP62ulmWVzU6qtTMNH6pZv7xaDvFfhA1+f9Pc8eNwq56G3dD0fdWsgro94u6FzaibdrdC3jb+IB9OofxIxOJz7FP8FqhvhnlSra7r4mL+w/eJTuochNwUH91qBE/zZloXKyHgD3kMH6KBOqBO34GfslIn+au6/KGPBomR4/Kiqot2vvg56f2H3ukZpaR+Lkh91jwnfm8+OKAxcF63eOlDoT3Pmvar1ewbEcvnxTRfNj8nfPxyPSkL5Pg+eBAqjM+KMn9d/MDSlyT80/33B1ZXCD6/qn/4FHmV/6J1QclWnZTjKbL+2ZLN+QdxzOv1YpHV1gb90Ab0Jrv44SeO3xStk7EfWq+69vND7/c5ORkfnig+a17WeZ3Tu40l3deF9ayE07XMZ8frdl7VP3wGsAg8zVf5khy/aZH/ELC4yekP4tKfNf+/G1zfKli47UiMI/UannDTFtONgczvH2veG0ak1VAQE2v6vhGYZcoNeNs2fWT1q0EMzffvi9bTgFU30TRoGKGm9/0wHf1G30QQ6M1wOOD/L4eD32g4+MEhIDyjps0Wqw92l85e/lyt6SLTd3zhrRD87PV8kzb+2VFcA9p3k477ELETbfMjkftZEjn8+/9yTr2FqRriya4p+xBGfGoMyvWPmPFniRl/DjL3GtO8ypbgwp/tzj9IDp6+j1s0qKVjvtNtxOK4aappwbgpzvHF3XDAp8tZequVXiF8d82Y6L0u22JVFlNC6bOPvtWj6E0dWDq4DmQVOAS8Mx7v9mCT9FLASNTIyhOaEQotimXbF/WCiLnKytug0Xn5tpoCs2L76X7zVGe0vQ2hb4OAeWcAEdtfR5ndRK3Hdz0W2sxZ4XL80ITHV99vmOjdLgkef7l8SkmBNk+Pp0CBlgOyZprN+sJLgjG7HS4Rpns/rv5anBelx20m/EM5Ljr423T8pvp/AZ/JMv7GiQ0X7m/gsE06yoMW4RF8cQO09xhcLCuia8RD6A2/Eht8pPX70GJDZ7dU2h9Mm1gQdAO6G1M5Pdr8kLTPbaK1Hm6v28wGfj8r+uhmit1GR3yocrqZNrfBwvdwf05UVTcYuokVekm8n2ve7IVqPYQ00v/ZZcYOXW4z998UB3YocJuu/1/DdqYrjj1uVEPR/OzPNQMGWA1zoW12/bPMiQE+PwfsaD736XGb/v/fwpPxNaQbuGDzGtH7OTc/u/x6w9LXjdL0zXLrRrrdhmu+Ia7dSJXb4BGB88PiZMmr0DstvZHXJouCT0kE8Xn+rjMIfed13noRC/lrLkUTuMc9Xzt82Y9BY0DCGPUGYBI29YFIEHPDy5FZiMGKCuHtQG8Ad1sQ/srIECy/ze2g2mz2EEjjBt0O3NMgKTgE07bqJ6E9Zo1zikvtpV7bCMsM5AAD5bY5C2jH2OPVnnjeBNIoSg+kyk5XFYcEuAVxwuxUhCgb0lcB5vEE1k0YbwARGfRGOn7dwavsD4w8klDp4xymVN5/zGESxXtfcfvggcYMjqZPIgPf0Hp4IMMvxQgT15kb6LQBvKXbNy8d1r2MrC8P0u3GRExsYJtSMX263YpWmzIofZAblP/Xp5u1D8PEiuYAosPpZgG+Llm6wXsfjkX7GyPE08CybWCdwdA0PsOx4PTr0iUANkyccCjfFIXiAdAtlZNtf+MgN4dM37SCsh0MU/N9SGhWM63Xbb97fPc1r4LrB4/vUpNpvmrXWfkFLeiWjfnii2y1ouVg87f7JH29yqY0jJPt17rGfrsF9oO7tMa+EBh3Q0XXjRFsT7TETyPvfEtdE6bPirppn2ZtNsmwQH0yW/Sa8Qc3xximNz/U6M+e8TBNa/wub7xYf563n2dlmdfXEpF03na0e0bDWeQU3WFkOi5rhvqv0Yuvp1mZ1Z1lcw7dTqpyvVhuCDOH3z9eFT0Q5rPbQ2Ef63g2q8nNCmGF39we4lfLabU8L+pFPhsGPtjo9v18O2vm+exl1jRXVT0LwXe/uz1UzOGSfuugaz+9PSQZV1lWV3kHu/Cb94R4InRjffGmepsvI7AjbW7fi6Haq7zJ20gPse8/BPrpu1UhCpQUQYfuN7fu9/z4bkdOu2pA1aqnBzoauatUbqVywmDla6ueII/x/ipo8+s/W6roSTXrAJBPbg/BxL49TIIvbg/vNREnBCSfvAeE9eSn82kXiPnw9nDeVNFxeR//v4aFh4LfW7IuZ8/en2Xjr/1ssSr+DQHIJ/+vmYSoJ/q15yQC7WtM0a2g/GzN2FnHep69l800QfFJte5qhM5X/2/jgA+f9a8/0z+82Y2wVg/kUJvb93JSra7r4mLe4QDv4/eAVefkeXSY0n54ezhP82ZaFysWppAv/S/eA94HcPoQzNN3LRzfUmfgq7rseJuR728PHV76cXlR1UU7X/QdeO+r94PZB/U+EM4oMukN1H74HnCa58QSTWcq3KfvD4lydpOyY726390e6vOsab9azfq8HHzxHvCKab5s+jzif357aC/Xk7JAENdRBe7j94Al3PmsKPPXxQ96IUbny/eAW1fwBnsj9j+/PbRX+S9aFxQEK72Op8joZEskqnzgG5rdvq/X68Uiqztq1n54ezhvsotOYC+fvAeEou2ytH50exg/Scmenha1H94eznMyLz1h0M9uD+WseVnTInuZS0orkFX/m9tDfFZmbZsv85lNVPtQ+99+DchPgyxvFHzY5P9t3tLGxZf3c5uGQd3egdoE42fZlRpyn94T2ptiQWYlW6y6wmk/vj2ss5fRJKD38e1hIWw+vsi7Lo738f/bWHN4yev9+FLgfH2eHHr//xv8iH9DOPLJ/9tm++nmJb33m3IL7PrrT/smGP/fmPp4BuA3vuXb6gi8ypYXHQYKv/nhM1K48hdyE/TZ7y8JuSEe8ZvElqJiiTWsYEYUqoAxC6N9QtyOSQYzmiCE7fa9MHojK6lfFyMAeU+Muguy7z1zsZXhL6+WWJC8Qdqjr9wiFxdbd4yQdbiXD5342yZo3pMPNiD8gXzxNTj1Zr7ordd3m1h9op/Yv+16va6Vf+Ev4vOgsCTPg2lWsm7fXTyXJh+lhPtlMcPC+etrilwWYzQYv/5F5UlZsHdkGnyRLYtzcuV4Ve2zj7C2/1F6XBZZg7WS8vyj9N2iXNIf87ZdPbp7t+EOmvGimNZVU52342m1uJvNqrv06sO7O3t389nibtPMSn9WPIvnCXdnEZ9mojsNZgpe5ef+HIbT9fhu9037nv8SOv/sowKDZ2H6nKKZGtmNl4hs6iUMTM5ofpS+WJclEimffXSelf0orQvfrLdLF+tl8YvWecHAzgsa5vuCC5fCBejyMqun86zeWmTv7vgQ23p9I8DBZfZvAHZ3jf0bAOmW2b8BYOEquwCcFO3Xm5TIivo3gGJsKf1nBWx31Vw6QYqvLUDuTfB9L2ejZA+vPf9/WMJlIfsbmJRgIdtD+P2gyCr212dmu4D9DYzIW8KOjieC0K15qe+7/n+Yh/Dv16D4rYl14/Ls/4dph9jvvSnXB9NZAfvG+TW6bvr/YbIPefS3JVwf4olba/0G5tOutw5bslthFay1fgN4fU0+6wOKrax+A/h11lW/IYjfCCC7xPpNwNJV0Q8xld2V1a8PKVhQ/UCG9VdTvwFCeeupH4hYbx3V0Ovi67D/S28J9RsY5oZF068/rXa99BtAUJZNvwlAsnj6DUCyC6jfACyzivr1iR2unX59OP3V0m9geANrpO8N+X3djaFVxv/vOx63cDY2S4FdpfxAteatUb73dPaheauU7w3tfZkjttz3854x8O8PgfZPB9bcft7T/xsK58KVu/cGeOvpjC+j3W4Wh9cY+m0H1qlunvX3zsD0Qdi+bwvi1tS71VLV7Yh56xWmW8/DzbT95mPg956tIVIfu6UoBe0nX39/k2vsUJqWtlLMti7CKBZY7BnLB1+sy7ZYlcWUuvzso53xeLc3IgcjSPf6sMIvQpjf6gGkiSK/jkaZIckOQhf9hfaXdUE+1Sorffw7jW6rSkFVC6/7zVNV3G1skLfpMEjz9ju28Du8eBMVgtXHzZzAq+eKctcvfE8O2O2tKn+5fEo+eJunx1N0SYmYrJlmsz7nYx31R5zzPpzjpdN/Tvgmttqunw2zUSz17M9o9PsfCpOZjGwEm58l1roxDa9vfdOcFs09c8sPsKM/RI77/U3SdDCc/Vqz+rPLVx6uEVz8b382ee1Wc/4N8dfQ7PA7g5z2c89dJtHz/3qW0oA9gof55v8vrBRLTXD7/xezkemKU3v/r+elpy4BEcHF//b/LzxlPu6mXPid/xfyFbvpGEHz+7+u1vW0m2NwU/pz6arbBEi3f/nwZ4V5fmguejy7w01vnUb44XPLm6y+yNtBbunNVmym/n/ALYZbbj2BP1RuQcOfO26JePm//5dXlAq+Udf8vz6e6yUTb8Aupiu/IT31cxzvbU6r8pv/34j/Ymx6g5L7uTSJ/+9jxFvP/s8l5/0w7ecpJ83pnZbeyGvF4KSa5c+KummfZm02yZq+FsRbr/PWw/ijVD7tsdrr6TxfZLSwPKlo5iWBj2+a3pR2wYZJzh748OtYN36Lm7sT89/rRj6OgWcv40awUUPQ6yXaKtZppOGtcRjud2Nft4bvJ3OGevLbbOjTb3a7zk3oP9Sx+X5DpyYBcssen3rB4VCvfpsNPdtmxS2o7aKKqMgNM6z59uYuBtXybRh3WO4jjcWM9RDy9FVcIbilu9RrG1ENA2t8EStpiWA+6Clg90ZMN/Gb4RddaxkO6xZD5nDGqbD+UMMG/18cYsy5cZqnP+KN7YeHs0EP86ii328gT0en+jC+OaLEMv7DJLlxfeDrDiD2Xl/Z+xA26O+vTwyrn4cpEM1hf5PDDs2M/7b55hsb7tPAKGyY9cGEqw7gGxm47cVaOx+C/+0HE8DL4WgUPqT4gkbfpPLr2lj7lnz4TQ5RI7jNQ4yFeQHCPWRjiP5QhxjT02Fu5ZbKfUNC5oeh4YecoCGYMd76Zsk3zDE3v/RNCskPh0DIMQCcjVLtd4/vipOpH9CfbVVTZ19Q/Fo2/CnFxmt6e5HLX0/zprhwIB4TzGXOKQ4H1LQ5W55XJjrvYGSamK91yr7I22xGIfNx3Rbn2bSlr6fkJhXLi4/Sn8zKNTtVk3x2tvxy3a7WLQ05X0zKQGUiyN/U/+O7PZwff7nCX803MQRCs6Ah5F8un6yLcmbxfpaVTUcpDIFA9uDznD6XucSk5xfXFtKLanlLQEq+pybp8SZfrEoC1ny5fJ1d5sO43UzDkGKPnxbZRZ0tfArKJ4rJ64x69rqgDvw3XH/0J7HrbPHu6P8JAAD//zGRy5h0pwAA"; }
        }
    }
}
