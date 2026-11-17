import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class IncomingVideoCallCard extends StatelessWidget {
  const IncomingVideoCallCard({super.key});

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Container(
      padding: EdgeInsetsDirectional.all(AppDimensions.spacing12.w),
      decoration: BoxDecoration(
        color: AppColors.ink,
        borderRadius: BorderRadius.circular(22.r),
      ),
      child: Row(
        children: [
          Container(
            width: 44.w,
            height: 44.w,
            decoration: const BoxDecoration(
              color: AppColors.callIconBg,
              shape: BoxShape.circle,
            ),
            alignment: Alignment.center,
            child: Icon(
              Icons.videocam_outlined,
              size: 22.sp,
              color: AppColors.callIconMint,
            ),
          ),
          SizedBox(width: AppDimensions.spacing12.w),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  s.chatMockupIncomingCallTitle,
                  style: AppTextStyles.label14WhiteBold,
                ),
                SizedBox(height: 2.h),
                Text(
                  s.chatMockupIncomingCallSubtitle,
                  style: AppTextStyles.caption11WhiteMutedRegular,
                ),
              ],
            ),
          ),
          Container(
            padding: EdgeInsetsDirectional.symmetric(
              horizontal: AppDimensions.spacing16.w,
              vertical: AppDimensions.spacing8.h,
            ),
            decoration: BoxDecoration(
              color: AppColors.callJoinGreen,
              borderRadius: BorderRadius.circular(20.r),
            ),
            child: Text(
              s.chatMockupJoinAction,
              style: AppTextStyles.caption13WhiteSemiBold,
            ),
          ),
        ],
      ),
    );
  }
}