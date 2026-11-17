import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/generated/l10n.dart';

class ChatMockupHeader extends StatelessWidget {
  const ChatMockupHeader({super.key});

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Row(
      children: [
        Container(
          width: 42.w,
          height: 42.w,
          decoration: BoxDecoration(
            color: AppColors.onPrimarySoft,
            shape: BoxShape.circle,
            border: Border.all(color: Colors.white, width: 1.5),
          ),
          alignment: Alignment.center,
          child: Text(
            s.chatMockupPharmacistInitials,
            style: AppTextStyles.body16PrimaryBold,
          ),
        ),
        SizedBox(width: AppDimensions.spacing12.w),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                s.chatMockupPharmacistName,
                style: AppTextStyles.label15InkBold,
              ),
              SizedBox(height: 2.h),
              Text(
                s.chatMockupActiveStatus,
                style: AppTextStyles.caption12SuccessSemiBold,
              ),
            ],
          ),
        ),
        Container(
          padding: EdgeInsetsDirectional.symmetric(
            horizontal: AppDimensions.spacing12.w,
            vertical: AppDimensions.spacing6.h,
          ),
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(20.r),
          ),
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(
                Icons.check_rounded,
                size: 14.sp,
                color: AppColors.primary,
              ),
              SizedBox(width: AppDimensions.spacing4.w),
              Text(
                s.chatMockupPaidStatus,
                style: AppTextStyles.caption12SuccessSemiBold,
              ),
            ],
          ),
        ),
      ],
    );
  }
}