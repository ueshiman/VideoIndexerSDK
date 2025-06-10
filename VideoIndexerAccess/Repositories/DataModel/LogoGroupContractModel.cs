using System;
using System.Collections.Generic;

namespace VideoIndexerAccess.Repositories.DataModel
{
    /// <summary>
    /// LogoGroupContract JSON�X�L�[�}�ɑΉ����郂�f��
    /// </summary>
    public class LogoGroupContractModel
    {
        /// <summary>
        /// �O���[�vID�iGUID�j
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// �쐬�����iISO 8601�`���j
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// �ŏI�X�V�����iISO 8601�`���j
        /// </summary>
        public DateTimeOffset LastUpdateTime { get; set; }

        /// <summary>
        /// �ŏI�X�V�ҁinull���e�j
        /// </summary>
        public string? LastUpdatedBy { get; set; }

        /// <summary>
        /// �쐬�ҁinull���e�j
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// ���S�����N�̃��X�g�inull���e�j
        /// </summary>
        public List<LogoGroupLinkModel>? Logos { get; set; }

        /// <summary>
        /// �O���[�v���inull���e�j
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// �O���[�v�̐����inull���e�j
        /// </summary>
        public string? Description { get; set; }
    }
}