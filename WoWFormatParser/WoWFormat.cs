namespace WoWFormatParser
{
    public enum WoWFormat : short
    {
        /// <summary>
        /// Unsupported/Invalid.
        /// </summary>
        Unsupported = -1,
        /// <summary>
        /// Terrain.
        /// </summary>
        ADT,
        /// <summary>
        /// Image.
        /// </summary>
        BLP,
        /// <summary>
        /// Shader.
        /// </summary>
        BLS,
        /// <summary>
        /// Day/Night Cycle.
        /// </summary>
        DB,
        /// <summary>
        /// Client Database.
        /// <para>Returns meta data only.</para>
        /// </summary>
        DBC,
        /// <summary>
        /// Map Name Definition.
        /// </summary>
        DEF,
        /// <summary>
        /// Lighting (≤ 1.9.0).
        /// </summary>
        LIT,
        /// <summary>
        /// Game Model (≥ 0.11.0).
        /// </summary>
        M2,
        /// <summary>
        /// Game Model (≤ 0.10.0).
        /// </summary>
        MDX,
        /// <summary>
        /// LUA Table of Contents.
        /// <para>Returns meta data only.</para>
        /// </summary>
        TOC,
        /// <summary>
        /// World Cache Database.
        /// <para>Returns meta data only.</para>
        /// </summary>
        WDB,
        /// <summary>
        /// World Level of Detail.
        /// </summary>
        WDL,
        /// <summary>
        /// World Table.
        /// </summary>
        WDT,
        /// <summary>
        /// Alpha WDT/ADT. All WDT and ADT information is contained in a single file.
        /// </summary>
        WDTADT,
        /// <summary>
        /// World Liquid.
        /// </summary>
        WLQ,
        /// <summary>
        /// World Liquid.
        /// </summary>
        WLM,
        /// <summary>
        /// World Liquid.
        /// </summary>
        WLX = WLM,
        /// <summary>
        /// World Liquid.
        /// </summary>
        WLW = WLM,
        /// <summary>
        /// World Model Object.
        /// </summary>
        WMO,
        /// <summary>
        /// World Model Object Group.
        /// </summary>
        WMOGROUP
    }
}
