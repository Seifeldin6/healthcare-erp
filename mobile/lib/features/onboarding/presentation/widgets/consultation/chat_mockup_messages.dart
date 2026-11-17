import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class ChatMockupMessages extends StatelessWidget {
  const ChatMockupMessages({super.key});

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Column(
      mainAxisSize: MainAxisSize.min,
      children: [
        Container(
          padding: EdgeInsetsDirectional.symmetric(
            horizontal: AppDimensions.spacing12.w,
            vertical: AppDimensions.spacing4.h,
          ),
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(14.r),
          ),
          child: Text(
            s.chatMockupPaymentBanner,
            style: AppTextStyles.caption11MutedMedium,
          ),
        ),
        SizedBox(height: AppDimensions.spacing12.h),
        Align(
          alignment: AlignmentDirectional.centerEnd,
          child: Container(
            constraints: BoxConstraints(maxWidth: 255.w),
            padding: EdgeInsetsDirectional.symmetric(
              horizontal: 14.w,
              vertical: 10.h,
            ),
            decoration: BoxDecoration(
              color: AppColors.primary,
              borderRadius: BorderRadiusDirectional.only(
                topStart: Radius.circular(18.r),
                topEnd: Radius.circular(18.r),
                bottomStart: Radius.circular(18.r),
                bottomEnd: Radius.circular(4.r),
              ),
            ),
            child: Text(
              s.chatMockupPatientMessage,
              style: AppTextStyles.caption13WhiteRegular,
            ),
          ),
        ),
        SizedBox(height: 10.h),
        Align(
          alignment: AlignmentDirectional.centerStart,
          child: Container(
            constraints: BoxConstraints(maxWidth: 265.w),
            padding: EdgeInsetsDirectional.symmetric(
              horizontal: 14.w,
              vertical: 10.h,
            ),
            decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadiusDirectional.only(
                topStart: Radius.circular(18.r),
                topEnd: Radius.circular(18.r),
                bottomEnd: Radius.circular(18.r),
                bottomStart: Radius.circular(4.r),
              ),
            ),
            child: Text(
              s.chatMockupPharmacistMessage,
              style: AppTextStyles.caption13InkRegular,
            ),
          ),
        ),
      ],
    );
  }
}