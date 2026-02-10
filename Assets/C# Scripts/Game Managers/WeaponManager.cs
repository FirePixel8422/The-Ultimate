


public static class WeaponManager
{
    public static SkillSet[] SkillSets { get; private set; }
    public static SkillSet ActiveWeapon_Local { get; private set; }


    public static void Init(GlobalWeaponListSO globalWeaponListSO)
    {
        int weaponCount = globalWeaponListSO.WeaponList.Length;
        SkillSets = new SkillSet[weaponCount];

        for (int i = 0; i < weaponCount; i++)
        {
            SkillSets[i] = globalWeaponListSO.WeaponList[i].GetAsSkillSet();
        }
    }
    public static void SwapToWeapon(int weaponId)
    {
        ActiveWeapon_Local = SkillSets[weaponId];
        SkillUIHandler.UpdateSkillUI(ActiveWeapon_Local);
    }
}