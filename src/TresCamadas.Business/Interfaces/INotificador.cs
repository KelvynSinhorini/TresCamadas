using TresCamadas.Business.Notificacoes;

namespace TresCamadas.Business.Interfaces;
public interface INotificador
{
    bool TemNotificacao();
    List<Notificacao> ObterNotificacoes();
    void Handle(Notificacao notificacao);
}