-UserlistComponent
    > Quand l'aministrateur modifie son profil sur la barre de menu, la liste des utilisateurs ne se met pas à jour
    > Quand on l'administrateur supprime un utilisateur, si ce dernier ne possède pas de tableau, il est supprimé, mais si il a un tableau, on a une erreur et la suppression ne se fait pas
-BoardlistComponent
    > Quand on utilise le bouton add new board de la barre de menu pour ajouter un tableau, la liste de tableaux ne se met pas à jour
-ShowBoardComponent
    > Quand on veut ajouter un participant à la carte, on a le choix sur tous les collaborateur du tableau alors que on devrait juste avoir les collaborateurs du tableau qui ne particpent pas sur la carte. On a utilisé la fonction "usersNotParticipantsCard(card: Card)" pour afficher uniquement ceux qui ne participe pas à la carte mais gros bug
-teamshowComponent
    > Quand on veux recuperer les users qui ne sont pas membre de la team
    > Quand on remove un user de la team sa ne refresh pas
-teamlistComponent
    > Quand on veux delete une team qui a des membre sa echou


### Fonctionnalités supplementaires
    - fonction admin
    - team
    - pas 2 listes avec le même titre dans le même tableau
    - plusieurs boutons pour rendre l'application plus ergonomique
    - l'implementation du fichier fr.ts et en.ts qui fonctionne bien, mais non complet
    - La possibilité d'utiliser des images pour les membres 

